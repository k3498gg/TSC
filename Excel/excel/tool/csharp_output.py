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
    ret, err = output_enum_csharp(pconf.code_path, pconf.enum_list, pconf.file_list)
    if ret != True:
        print("output_enum_define failed, err:%s." % (err,))
        return ret, err

    for fen in pconf.file_list:
        ret, err = output_entry_csharp(pconf.code_path, fen, pconf.enum_list)
        if ret != True:
            print("entry %s output cs failed, err:%s." % (fen.target_name, err))
            return ret, err
        else:
            print("entry %s output cs ok." % (fen.target_name,))

    return True, ""


# 输出单个entry cpp代码
def output_entry_csharp(code_path, fconf, enum_list):
    hstr  = 'using System;\n'
    hstr += 'using System.IO;\n'
    hstr += 'using System.Collections.Generic;\n'
    hstr += 'using System.Text;\n'
    hstr += '\n\n'
    enum_str = output_entry_csharp_enum(fconf, enum_list)
    if enum_str != '':
        hstr += enum_str

    hstr += '[Serializable]\n'
    hstr += 'public class %s: IInfo\n' % (fconf.target_name,)
    hstr += '{\n'

    # 填充字段
    for col in fconf.column_list:
        if col.count <= 1:
            if col.type == "Number":
                if col.refer_enum != "":
                    hstr += '    public %s %s; // %s\n' % (col.refer_enum, col.name, col.column)
                else:
                    hstr += '    public int %s; // %s\n' % (col.name, col.column)
            elif col.type == "String":
                hstr += '    public string %s; // %s\n' % (col.name, col.column)
            else:
                return False, "fconf(%s) not support column type(%s)." % (col.name, col.type)
        else:
            if col.type == "Number":
                if col.refer_enum != "":
                    hstr += '    public %s[] %s; // %s\n' % (col.refer_enum, col.name, col.column)
                else:
                    hstr += '    public int[] %s; // %s\n' % (col.name, col.column)
            elif col.type == "String":
                hstr += '    public string[] %s; // %s\n' % (col.name, col.column)
            else:
                return False, "fconf(%s) not support column type(%s)." % (col.name, col.type)

    hstr += '\n'
    hstr += '    public  void Load(BinaryReader reader)\n'
    hstr += '    {\n'
    for col in fconf.column_list:
        if col.count <= 1:
            if col.type == 'Number':
                if col.refer_enum != "":
                    hstr += '        %s = (%s)reader.ReadInt32();\n' % (col.name, col.refer_enum)
                else:
                    hstr += '        %s = reader.ReadInt32();\n' % (col.name,)
            elif col.type == 'String':
                hstr += '        int %sLen = reader.ReadInt32();\n' % (col.name,)
                hstr += '        %s = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(%sLen));\n' % (col.name, col.name)
        else:
            if col.type == 'Number':
                if col.refer_enum != "":
                    hstr += '        %s = new %s[%d];\n' % (col.name, col.refer_enum, col.count)
                else:
                    hstr += '        %s = new int[%d];\n' % (col.name, col.count)
            elif col.type == 'String':
                hstr += '        %s = new string[%d];\n' % (col.name, col.count)
            hstr += '        for(int i = 0; i < %d; ++i)\n' % (col.count,)
            hstr += '        {\n'
            if col.type == 'Number':
                if col.refer_enum != "":
                    hstr += '            %s[i] = (%s)reader.ReadInt32();\n' % (col.name, col.refer_enum)
                else:
                    hstr += '            %s[i] = reader.ReadInt32();\n' % (col.name,)
            elif col.type == 'String':
                hstr += '            int %sLen = reader.ReadInt32();\n' % (col.name,)
                hstr += '            %s[i] = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(%sLen));\n' % (col.name, col.name)
            hstr += '        }\n'
        hstr += ' \n'
        #hstr += '        return True;\n'
    hstr += '    }\n'
    hstr += '\n'
    hstr += '    public  int GetKey()\n'
    hstr += '    {\n'
    for col in fconf.column_list:
        if col.is_key == True:
            hstr += '        return %s;\n' % (col.name,)
    hstr += '    }\n'
    hstr += '\n'
    hstr += '}\n'
    
    try:
        fout = open(os.path.join(code_path, fconf.target_name + ".cs"), "w")
        fout.write(hstr)
        fout.close()
    except Exception as e:
        return False, "open cpp header failed:" + str(e)

    return True, ""

# 输出枚举定义
def output_entry_csharp_enum(fconf, enum_list):
    hstr = ''
    for enum_conf in enum_list:
        if fconf.target_name == enum_conf.target_name:
            hstr += '// %s\n' % (enum_conf.desc,)
            hstr += 'public enum %s\n' % (enum_conf.name,)
            hstr += '{\n'
            for enum_field in enum_conf.field_list:
                hstr += '    %s = %d,  //%s\n' % (enum_field.name, int(enum_field.value), enum_field.desc)
            hstr += '};\n'
            hstr += '\n'
    return hstr


# 输出枚举定义
def output_enum_csharp(code_path, enum_list, file_list):
    target_dic = {}
    for enum_conf in enum_list:
        #print(enum_conf.name, enum_conf.desc)
        ret = check_target_name_in_file_list(enum_conf.target_name, file_list)
        #print(ret)
        if ret == False:
            if target_dic.has_key(enum_conf.target_name):
                target_dic[enum_conf.target_name].append(enum_conf)
            else:
                target_dic[enum_conf.target_name] = []
                target_dic[enum_conf.target_name].append(enum_conf)

    for k, v in target_dic.items():
        cstr = '\n\n'
        for enum_conf in v:
            cstr += '// %s\n' % (enum_conf.desc,)
            cstr += 'public enum %s\n' % (enum_conf.name,)
            cstr += '{\n'
            for field in enum_conf.field_list:
                cstr += '    %s = %d, //%s\n' % (field.name, int(field.value), field.desc)
            cstr += '};\n'
            cstr += '\n'
        cstr += '\n'
        #cstr += '#endif\n'
        try:
            fo = open(os.path.join(code_path, '%s.cs' % (k,)), 'w')
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
