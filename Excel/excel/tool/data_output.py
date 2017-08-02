#!/usr/bin/python
# -*- coding:utf-8 -*-

import os
import sys
import xlrd
import struct
import traceback

import pro_conf

reload(sys)
sys.setdefaultencoding("utf-8")


class OutputError:
    def __init__(self):
        self.output_err_file = ""
        self.output_err_sheet = ""
        self.output_err_line = -1
        self.output_err_msg = ""
    # 
    def get_err_msg(self):
        strmsg = "error:\r\n"
        if self.output_err_file != "":
            strmsg += ("file: " + self.output_err_file + "\r\n")
        if self.output_err_sheet != "":
            strmsg += ("sheet_name:" + self.output_err_sheet + "\r\n")
        if self.output_err_line >= 0:
            strmsg += ("line: " + str(self.output_err_line) + "\r\n")
        if self.output_err_msg != "":
            strmsg += ("err: " + self.output_err_msg + "\r\n")
        return strmsg
    
    def reset_err_msg(self):
        self.output_err_file = ""
        self.output_err_sheet = ""
        self.output_err_line = -1
        self.output_err_msg = ""


# 替换功能实现
replace_xls_data = {}
def get_replace_value(xls_path, value_str, target_str):
    #print("get_replace_value:" + target_str)
    target = target_str.split(":")
    xls_name = target[0]
    sheet_name = target[1]
    key_col = target[2]
    value_col = target[3]

    # 先查找是否已有该表格数据
    t_key = xls_name + "_" + sheet_name
    if replace_xls_data.has_key(t_key):
        v_data = replace_xls_data[t_key]
        if v_data.has_key(value_str):
            return True, v_data[value_str]

    # 没有新读表格
    xls_file = os.path.join(xls_path, xls_name)
    data = xlrd.open_workbook(xls_file)
    table = data.sheet_by_name(sheet_name)
    if table.nrows <= 1:
        return False, ""

    # 计算col对应索引
    col_key = -1
    col_value = -1
    for i in range(0, table.ncols):
        col_name = str(table.cell(0, i).value)
        if col_name == key_col:
            col_key = i
        if col_name == value_col:
            col_value = i
    if col_key == -1 or col_value == -1:
        return False, ""

    table_data = {}
    result_str = ""
    for i in range(1, table.nrows):
        key_str = str(table.cell(i, col_key).value)
        v_str = str(table.cell(i, col_value).value)
        table_data[key_str] = v_str
        if key_str == value_str:
            result_str = v_str

    replace_xls_data[t_key] = table_data

    if result_str != "":
        return True, result_str

    return False, ""

# 替换refer_num字段
def get_replace_refer_enum_value(value, enum_name, enum_list):
    for enum in enum_list:
        if enum.name == enum_name:
            for field in enum.field_list:
                if field.desc == value:
                    return True, field.value
    return False, ''

# 读取所有xls数据
# xls读出格式， <xls_file+sheet_name>->data{}
# data格式  <header->value>{}
global_xls_data = {}
global_xls_dic = {}
def read_all_xls_data(pconf):
    err = OutputError()
    for fen in pconf.file_list:
        #print("read_all_xls_data fconf:" + fen.target_name)
        f_path = os.path.join(pconf.xls_path, fen.xls_file)
        err.output_err_file = f_path
        err.output_err_sheet = fen.sheet_name
        try:
            data = xlrd.open_workbook(f_path)
            table = data.sheet_by_name(fen.sheet_name)
            if table.nrows <= (int(pconf.data_start_line) - 1):
                err.output_err_msg = "table row line <= 1, no data."
                return False, err
            table_data = []
            table_dic = {}
            for i in range((int(pconf.data_start_line) - 1), table.nrows):
                
                err.output_err_line = i
                row_dic = {}
                for col in range(0, table.ncols):
                    key = table.cell(0, col).value
                    
                    # 检查key是否在fen配置中配置
                    if check_in_file_column_list(key, fen) == False:
                        continue
                    value = table.cell(i, col).value
                    if table.cell(i, col).ctype == 2:
                        value = int(value)
                    value = str(value)
                    #value = str(table.cell(i, col).value)
                    #print("value:" + value + ", type:" + str(table.cell(i,col).ctype))
                    # 如果单元格类型为2(整数)，强转为int->string
                    row_dic[key] = value

                col_name = get_file_key_column_name(fen)
                if col_name == "":
                    err.output_err_msg = "fconf(%s) key column not found." % (fen.target_name,)
                    return False, err
                try:
                    row_key = int(float(row_dic[col_name]))
                except Exception as e:
                    err.output_err_msg = "fconf(%s) key get failed, col_name(%s) value(%s)" % (fen.target_name, col_name, str(row_dic[col_name]))
                    return False, err

                table_data.append(row_dic)
                if table_dic.has_key(row_key):
                    err.output_err_msg = "fconf(%s) key(%s) duplicated." % (fen.target_name, row_dic[col_name])
                    return False, err
                table_dic[row_key] = row_dic

            xls_key = fen.xls_file + "_" + fen.sheet_name
            global_xls_data[xls_key] = table_data
            global_xls_dic[xls_key] = table_dic
        except Exception as e:
            err.output_err_msg = "file:%s sheet_name:%s exception,e:%s" % (str(f_path), str(fen.sheet_name), str(e))
            return False, err

    return True, None 

# 检测col_name是否在f_conf column_list定义
def check_in_file_column_list(col_name, fconf):
    for col in get_column_list(fconf):
        #print(col)
        if col[0] == col_name:
            return True
    
    return False

# 查找f_conf key column
def get_file_key_column_name(fconf):
    for col_conf in fconf.column_list:
        if col_conf.is_key == True:
            return col_conf.column
    return ""

# 获取特定表格对应的数据
def get_xls_data(xls_file, sheet_name):
    key = xls_file + "_" + sheet_name
    try:
        return global_xls_data[key]
    except Exception as e:
        return None

def get_xls_dic(xls_file, sheet_name):
    key = xls_file + "_" + sheet_name
    try:
        return global_xls_dic[key]
    except Exception as e:
        return None


# 输出数据
def output(pconf):
    err = OutputError()
    if len(pconf.file_list) == 0:
        err.output_err_msg = "project conf file entry empty."
        return False, err

    ret, oerr = read_all_xls_data(pconf)
    if ret != True:
        return False, oerr

    #print("read_all_xls_data ok .")

    for f in pconf.file_list:
        err.reset_err_msg()

        f_path = os.path.join(pconf.xls_path, f.xls_file)
        err.output_err_file = f_path
        err.output_err_sheet = f.sheet_name
        
        #of_file_name = f.target_name + ".bin"
        #of_path = os.path.join(pconf.data_path, of_file_name)
        if pconf.data_type == "bin":
            bin_ret, oerr = output_bin(f, pconf.data_path, pconf.xls_path)
            #print("hello")
            if bin_ret != True:
                print("%s,%s output_bin failed, err:\r\n" % (f.xls_file, f.sheet_name))
                print(oerr.get_err_msg())
                return False, oerr
            else:
                print("%s,%s output_bin ok." % (f.xls_file, f.sheet_name))
        else:
            err.output_err_msg = "project data_type not support:" + pconf.data_type
            return False, err
        #print(f.target_name + " output ok.")
    
    #print("output ok.")
    return True, None

# 获取FileConf对应的column列表
# 兼容转换数组结构
def get_column_list(fconf):
    column_list = []
    for col in fconf.column_list:
        if col.count <= 1:
            col_data = (col.column, col.type, col.refer_enum, col.check_list, col.name)
            column_list.append(col_data)
        else:
            for i in range(1, col.count+1):
                col_data = (col.column + "#" + str(i), col.type, col.refer_enum, col.check_list, col.name)
                column_list.append(col_data)

    return column_list

# 输出二进制格式数据
# 文件格式(count(4bytes), row(number(4bytes uint), string(count(4bytes), str))
def output_bin(fconf, data_path, xls_path):
    err = OutputError()
    table = get_xls_data(fconf.xls_file, fconf.sheet_name)
    if table == None:
        err.output_err_msg = "not found xls data."
        return False, err

    fout_path = os.path.join(data_path, fconf.target_name + ".bin")
    try:
        fout = open(fout_path.lower(), "wb")
        #输出4个字节uint,行数
        data = struct.pack('I', len(table))
        fout.write(data)

        col_list = get_column_list(fconf)
        
        err_msg = ""

        #输出每行内容
        row_line = 0
        for row in table:
            row_line = row_line + 1
            err.out_err_line = row_line
            #print("col_list:" + str(col_list))
            #cur_row = str(col_list)
            for col in col_list:
                if not row.has_key(col[0]):
                    err.output_err_msg = "no column:" + col[0]
                    return False, err
                value = row[col[0]]
                #print(value)
                err_msg = "column:" + str(col[4]) + "," + value
                # 检查是否需要替换字符串
                if col[2] != "":
                    #print("col:" + str(col))
                    #replace_ret, nvalue = get_replace_value(xls_path, value, col[2])
                    replace_ret, nvalue = get_replace_refer_enum_value(value, col[2], pconf.enum_list)
                    if replace_ret != True:
                        err.output_err_msg = "replace refer_enum(%s) value(%s) failed." % (col[2], value)
                        return False, err
                    #print("old:%s n:%s" % (value, nvalue))
                    value = nvalue

                # 检查该项是否符合检查条件
                if value != "":
                    ret, check_err = check_cell(value, col[3], pconf)
                    if ret != True:
                        err.output_err_msg = "line(%d) column(%s) value(%s) check failed, %s" % (row_line+2, col[0], value, check_err)
                        return False, err

                if col[1] == "Number":
                    if value.strip() == "":
                        value = "0.0"
                    #print("TT:" + value + ":tt" + ",line:" + row_line);
                    try:
                        value = int(float(value))
                    except Exception as e:
                        err.output_err_line = row_line
                        err.output_err_msg = "exception e:%s,%s" % (str(e), err_msg)
                        return False, err

                data = ""
                if col[1] == "Number":
                    data = struct.pack('i', value)
                elif col[1] == "String":
                    data = struct.pack("i" + str(len(value)) + "s", len(value), value)
                    #print("column(%s) cell(%s)" % (col[0], value))
                else:
                    err.output_err_msg = "column type(%s) not support." % (col[1],)
                    return False, err
                fout.write(data)

        fout.close()
    except Exception as e:
        #err.output_err_msg = "write file failed, path:" + fout_path
        err.output_err_msg = "exception e:%s,%s" % (str(e), err_msg)
        print("write file failed, path:" + fout_path)
        traceback.print_exc()
        return False, err

    #print("write ok.")
    return True, None


# 检查表格配置条件
def check_cell(value, check_list, pconf):
    for check in check_list:
        if check.type == "num_limit":
            min_v = int(check.target.split(":")[0])
            max_v = int(check.target.split(":")[1])
            v = int(float(value))
            if v < min_v or v > max_v:
                return False, check.target
        elif check.type == "ref_table":
            fconf = None    
            for f in pconf.file_list:
                if f.target_name == check.target:
                    fconf = f
                    break
            if fconf == None:
                return False, check.target

            # 关联表格允许空，或者0
            if value == "" or value == "0":
                return True, ""
                
            xls_file = fconf.xls_file
            sheet_name = fconf.sheet_name
            table_dic = get_xls_dic(xls_file, sheet_name)
            v_key = int(float(value))
            if table_dic.has_key(v_key) == False:
                return False, check.target

    return True, ""

if __name__ == "__main__":
    #print(len(sys.argv))
    conf_file = "./pro_conf.xml"
    data_out = ""
    if len(sys.argv) > 1:
        conf_file = sys.argv[1]
    if len(sys.argv) > 2:
        data_out = sys.argv[2]
    print(conf_file)
    print(data_out)
    pconf, err = pro_conf.parse_pro_conf(conf_file)
    if pconf == None:
        print("pro_conf parse_pro_conf failed.")
        sys.exit(-1)
        
    # 替换数据输出目录
    if data_out != "":
        pconf.data_path = data_out

    oret, err = output(pconf)
    if oret != True:
        print("pro_conf output failed:" + err.get_err_msg())
        sys.exit(-1)
    
    print("pro_conf output ok.")
