#!/usr/bin/python
# -*- coding:utf-8 -*-

import os
import sys

import pro_conf

import cpp_output
import csharp_output

reload(sys)
sys.setdefaultencoding("utf-8")


# 输出代码
def output(pconf):
    if pconf.data_type == "bin":
        if pconf.code_type == "c++":
            cpp_output.output(pconf)
        elif pconf.code_type == "c#":
            csharp_output.output(pconf)
        else:
            err_msg = "project conf code_type not support:" + pconf.code_type
            return False, err_msg
    else:
        err_msg = "project conf data_type not support:" + pconf.data_type
        return False, err_msg

    return True, ""


if __name__ == "__main__":
    #print(len(sys.argv))
    conf_file = "./pro_conf.xml"
    if len(sys.argv) > 1:
        conf_file = sys.argv[1]
    print(conf_file)
    pconf, err = pro_conf.parse_pro_conf(conf_file)
    if pconf == None:
        print("pro_conf parse failed, err:" + err)
        sys.exit(-1)

    ret, err = output(pconf)
    if ret != True:
        print("pro_conf code_output failed:" + err)
        sys.exit(-1)

    print("pro_conf code_output ok.")
