#!/usr/bin/python
# -*- coding:utf-8 -*-

import os
import sys

import pro_conf

reload(sys)
sys.setdefaultencoding("utf-8")


# 输出代码
def output(pconf):
    # 输出独立的枚举定义文件
    ret, err = output_enum_define(pconf.code_path, pconf.enum_list, pconf.file_list)
    if ret != True:
        print("output_enum_define failed, err:%s." % (err,))
        return ret, err

    for fen in pconf.file_list:
        ret, err = output_entry_cpp(pconf.code_path, fen, pconf.enum_list)
        if ret != True:
            print("entry %s output cpp failed, err:%s." % (fen.target_name, err))
            return ret, err
        else:
            print("entry %s output cpp ok." % (fen.target_name,))

    return True, ""


# 输出单个entry cpp代码
def output_entry_cpp(code_path, fconf, enum_list):
    # 输出头文件
    ret, err = output_entry_cpp_header(code_path, fconf, enum_list)
    if ret != True:
        print("entry %s output cpp header failed, err:%s." % (fconf.target_name,err))
        return ret, err
    ret, err = output_entry_cpp_code(code_path, fconf)
    if ret != True:
        print("entry %s output cpp code failed, err:%s." % (fconf.target_name, err))
        return ret, err

    return True, ""

# 输出枚举定义
def output_entry_cpp_enum(fconf, enum_list):
    hstr = ''
    for enum_conf in enum_list:
        if fconf.target_name == enum_conf.target_name:
            hstr += '// %s\n' % (enum_conf.desc,)
            hstr += 'enum %s\n' % (enum_conf.name,)
            hstr += '{\n'
            for enum_field in enum_conf.field_list:
                hstr += '    %s = %d,  //%s\n' % (enum_field.name, int(enum_field.value), enum_field.desc)
            hstr += '};\n'
            hstr += '\n'
    return hstr
    
# 输出常量定义
def output_entry_cpp_const(fconf):
    hstr = ''
    const_dic = {}
    for col in fconf.column_list:
        if col.count > 1 and col.count_const_value != "":
            try:
                const_dic[col.count_const_value] = col.count
            except:
                pass
    for k, v in const_dic.items():
        hstr += '    static const int %s = %d;\n' % (k, v)
    return hstr

# 输出cpp头文件
def output_entry_cpp_header(code_path, fconf, enum_list):
    hstr  = "#ifndef __%s_H__\n" % (fconf.target_name,)
    hstr += "#define __%s_H__\n" % (fconf.target_name,)
    hstr += "\n"
    hstr += '#include "InfoMgr.h"\n'
    hstr += '#include <string>\n'
    hstr += '\n'
    enum_str = output_entry_cpp_enum(fconf, enum_list)
    if enum_str != "":
        hstr += enum_str
    hstr += 'class %s\n' % (fconf.target_name,)
    hstr += '{\n'
    hstr += 'public:\n'
    const_str = output_entry_cpp_const(fconf)
    if const_str != "":
        hstr += const_str
        hstr += '\n'
    hstr += '    %s();\n' % (fconf.target_name,)
    hstr += '    virtual ~%s();\n' % (fconf.target_name,)
    hstr += '\n'
    hstr += '    //\n'
    hstr += '    virtual bool Load(InfoStream& stream);\n'
    hstr += '\n'
    hstr += '    //\n'
    hstr += '    virtual bool Check();\n'
    hstr += '\n'
    hstr += '    //\n'
    hstr += '    int GetKey()const;\n'
    hstr += '\n'
    hstr += '    //\n'
    hstr += '    std::string ToString()const;\n'
    hstr += '\n'
    hstr += 'public:\n'
    # 填充字段
    for col in fconf.column_list:
        # 特殊处理主键字段
        if col.is_key == True:
            hstr += '    union\n'
            hstr += '    {\n'
            hstr += '        int infoId; // 配置id\n'
            hstr += '        int %s;     // %s\n' % (col.name, col.column)
            hstr += '    };\n'
            hstr += '    int index; // 索引\n'
            continue
        if col.count <= 1:
            if col.type == "Number":
                hstr += '    int %s; // %s\n' % (col.name, col.column)
            elif col.type == "String":
                hstr += '    std::string %s; // %s\n' % (col.name, col.column)
            else:
                return False, "fconf(%s) not support column type(%s)." % (col.name, col.type)
        else:
            if col.count_const_value == '':
                if col.type == "Number":
                    hstr += '    int %s[%d]; // %s\n' % (col.name, col.count, col.column)
                elif col.type == "String":
                    hstr += '    std::string %s[%d]; // %s\n' % (col.name, col.count, col.column)
                else:
                    return False, "fconf(%s) not support column type(%s)." % (col.name, col.type)
            else:
                if col.type == "Number":
                    hstr += '    int %s[%s]; // %s\n' % (col.name, col.count_const_value, col.column)
                elif col.type == "String":
                    hstr += '    std::string %s[%s]; // %s\n' % (col.name, col.count_const_value, col.column)
                else:
                    return False, "fconf(%s) not support column type(%s)." % (col.name, col.type)

    hstr += '};\n'
    hstr += '\n'
    hstr += 'typedef InfoMgr<%s> %sMgr;\n' % (fconf.target_name, fconf.target_name)
    hstr += '\n'
    hstr += '#endif\n'
    try:
        fout = open(os.path.join(code_path, fconf.target_name + ".h"), "w")
        fout.write(hstr)
        fout.close()
    except Exception as e:
        return False, "open cpp header failed:" + str(e)

    return True, ""

# 输出源文件
def output_entry_cpp_code(code_path, fconf):
    cstr  = '#include "%s.h"\n' % (fconf.target_name,)
    cstr += '\n'
    cstr += '#include <sstream>\n'
    cstr += '\n'
    # 包含依赖的头文件
    for col in fconf.column_list:
        for check in col.check_list:
            if check.type == "ref_table":
                cstr += '#include "%s.h"\n' % (check.target)
    cstr += '\n'

    # 构造函数
    cstr += '%s::%s()\n' % (fconf.target_name, fconf.target_name)
    cstr += '{\n'
    cstr += '    index = 0;\n'
    for col in fconf.column_list:
        if col.count <= 1:
            if col.type == "Number":
                cstr += '    %s = 0;\n' % (col.name,)
        else:
            if col.type == "Number":
                cstr += '    for(int i = 0; i < %d; ++i)\n' % (col.count,)
                cstr += '    {\n'
                cstr += '        %s[i] = 0;\n' % (col.name,)
                cstr += '    }\n'
    cstr += '}\n'
    cstr += '\n'

    # 析够函数
    cstr += '%s::~%s()\n' % (fconf.target_name, fconf.target_name)
    cstr += '{\n'
    cstr += '}\n'
    cstr += '\n'

    # 加载函数
    cstr += 'bool %s::Load(InfoStream& stream)\n' % (fconf.target_name,)
    cstr += '{\n'
    for col in fconf.column_list:
        if col.count <= 1:
            cstr += '    stream.Read(%s);\n' % (col.name,)
        else:
            cstr += '    for(int i = 0; i < %d; ++i)\n' % (col.count,)
            cstr += '    {\n'
            cstr += '        stream.Read(%s[i]);\n' % (col.name,)
            cstr += '    }\n'
    cstr += '\n'
    cstr += '    return true;\n'
    cstr += '}\n'
    cstr += '\n'

    # 检查函数
    cstr += 'bool %s::Check()\n' % (fconf.target_name,)
    cstr += '{\n'
    for col in fconf.column_list:
        if col.count <= 1:
            for check in col.check_list:
                if check.type == "num_limit":
                    min_v = int(check.target.split(':')[0])
                    max_v = int(check.target.split(':')[1])
                    cstr += '    if(%s < %d || %s > %d)\n' % (col.name, min_v, col.name, max_v)
                    cstr += '    {\n'
                    cstr += '        return false;\n'
                    cstr += '    }\n'
                elif check.type == "ref_table":
                    cstr += '    if(%s > 0 && %sMgr::GetInstance().GetById(%s) == NULL)\n' % (col.name, check.target, col.name)
                    cstr += '    {\n'
                    cstr += '        return false;\n'
                    cstr += '    }\n'
        else:
            for check in col.check_list:
                if check.type == "num_limit":
                    min_v = int(check.target.split(':')[0])
                    max_v = int(check.target.split(':')[1])
                    cstr += '    for(int i = 0; i < %d; ++i)\n' % (col.count,)
                    cstr += '    {\n'
                    cstr += '        if(%s[i] < min_v || %s[i] > max_v)\n' % (col.name, col.name)
                    cstr += '        {\n'
                    cstr += '            return false;\n'
                    cstr += '        }\n'
                    cstr += '    }\n'
                elif check.type == "ref_table":
                    cstr += '    for(int i = 0; i < %d; ++i)\n' % (col.count,)
                    cstr += '    {\n'
                    cstr += '        if(%sMgr::GetInstance().GetById(%s[i]) == NULL)\n' % (check.target, col.name)
                    cstr += '        {\n'
                    cstr += '            return false;\n'
                    cstr += '        }\n'
                    cstr += '    }\n'

    cstr += '    return true;\n'
    cstr += '}\n'
    cstr += '\n'

    # 获取Key函数
    cstr += 'int %s::GetKey()const\n' % (fconf.target_name,)
    cstr += '{\n'
    for col in fconf.column_list:
        if col.is_key == True:
            cstr += '    return %s;\n' % (col.name,)

    cstr += '}\n'
    cstr += '\n'

    # 字符串函数
    cstr += 'std::string %s::ToString()const\n' % (fconf.target_name,)
    cstr += '{\n'
    cstr += '    std::ostringstream oss;\n'
    for col in fconf.column_list:
        if col.count <= 1:
            cstr += '    oss<<"%s:"<<%s<<std::endl;\n' % (col.name, col.name)
        else:
            cstr += '    for(int i = 0; i < %d; ++i)\n' % (col.count,)
            cstr += '    {\n'
            cstr += '        oss<<"%s["<<i<<"]:"<<%s[i]<<std::endl;\n' % (col.name, col.name)
            cstr += '    }\n'
    cstr += '    return oss.str();\n'
    cstr += '}\n'
    cstr += '\n'

    try:
        fout = open(os.path.join(code_path, fconf.target_name + ".cpp"), "w")
        fout.write(cstr)
        fout.close()
    except Exception as e:
        return False, "open cpp code failed:" + str(e)

    return True, ""

# 输出枚举定义
def output_enum_define(code_path, enum_list, file_list):
    target_dic = {}
    for enum_conf in enum_list:
        ret = check_target_name_in_file_list(enum_conf.target_name, file_list)
        if ret == False:
            if target_dic.has_key(enum_conf.target_name):
                target_dic[enum_conf.target_name].append(enum_conf)
            else:
                target_dic[enum_conf.target_name] = []
                target_dic[enum_conf.target_name].append(enum_conf)

    for k, v in target_dic.items():
        cstr  = '#ifndef __%s_H__\n' % (k,)
        cstr += '#define __%s_H__\n' % (k,)
        cstr += '\n\n'
        for enum_conf in v:
            cstr += '// %s\n' % (enum_conf.desc,)
            cstr += 'enum %s\n' % (enum_conf.name,)
            cstr += '{\n'
            for field in enum_conf.field_list:
                cstr += '    %s = %d, //%s\n' % (field.name, int(field.value), field.desc)
            cstr += '};\n'
            cstr += '\n'
        cstr += '\n'
        cstr += '#endif\n'
        try:
            fo = open(os.path.join(code_path, '%s.h' % (k,)), 'w')
            fo.write(cstr)
            fo.close()
        except Exception as e:
            return False, 'write enum(%s) failed, %s' % (k, e.args)
    
    return True, ''
        
# 检查proconf中是否有target_name对应的file_entry定义
def check_target_name_in_file_list(target_name, file_list):
    for fen in file_list:
        if fen.target_name == target_name:
            return True
    return False

if __name__ == '__main__':
    pconf, err = pro_conf.parse_pro_conf("./pro_conf.xml")
    if pconf == None:
        print('pconf parsed failed, err:' + err)
        sys.exit(-1)

    output(pconf)
