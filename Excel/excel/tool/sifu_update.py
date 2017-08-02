# -*- coding:utf-8 -*-
_author__ = 'Administrator'

import os
import sys
import requests
import tarfile
import shutil
import time

SERVER_URL = 'http://192.168.1.245:8000/'
REQUEST_TIMEOUT = 100

# 打包目录
def make_tar(path, dst_path, comp='bz2'):
    if comp:
        d_ext = '.' + comp
    else:
        d_ext = ''

    arcname = os.path.basename(path)
    dest_name = '%s.tar%s' % (arcname, d_ext)
    dest_path = os.path.join(dst_path, dest_name)
    if comp:
        dest_cmp = ':'+comp
    else:
        dest_cmp = ''

    out = tarfile.TarFile.open(dest_path, 'w'+dest_cmp)
    out.add(path, arcname)
    out.close()
    return dest_path

# 清空上传目录
def clear_upload_path(user):
    url = SERVER_URL + '/sifu_upload_clear_' + user
    try:
        r = requests.get(url, timeout=REQUEST_TIMEOUT)
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'clear except:' + str(e)
    return True, ''

# 上传bin配置文件
def upload_bin_files(user):
    svr_bin_path = './excel/data_out/server'
    upload_url = SERVER_URL + '/sifu_upload_' + user
    try:
        make_tar(svr_bin_path, './', '')
    except Exception as e:
        return False, 'tar except:' + str(e)

    # 上传文件
    bin_tar_ofile = open('./server.tar', 'rb+')
    files = {'file': bin_tar_ofile}
    try:
        r = requests.post(upload_url, files=files, timeout=REQUEST_TIMEOUT*3)
        bin_tar_ofile.close()
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'upload except:' + str(e)

    # 删除tar
    try:
        os.remove('./server.tar')
    except Exception as e:
        return False, 'rm tar except:' + str(e)

    return True, ''

# 上传脚本
def upload_script_files(user):
    svr_script_path = './script'
    upload_url = SERVER_URL + '/sifu_upload_' + user
    try:
        make_tar(svr_script_path, './', '')
    except Exception as e:
        return False, 'tar except:' + str(e)

    # 上传文件
    script_tar_ofile = open('./script.tar', 'rb+')
    files = {'file': script_tar_ofile}
    try:
        r = requests.post(upload_url, files=files, timeout=REQUEST_TIMEOUT*3)
        script_tar_ofile.close()
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'upload except:' + str(e)

    # 删除tar
    try:
        os.remove('./script.tar')
    except Exception as e:
        return False, 'rm tar except:' + str(e)

    return True, ''

# 上传地图配置文件
def upload_map_file(user):
    svr_map_path = './map2xml/mapconf'
    # 上传所有map配置文件
    upload_url = SERVER_URL + '/sifu_upload_' + user
    try:
        make_tar(svr_map_path, './', '')
    except Exception as e:
        return False, 'tar except:' + str(e)
        
    map_tar_ofile = open('./mapconf.tar', 'rb+')
    files = {'file': map_tar_ofile}
    try:
        r = requests.post(upload_url, files=files, timeout=REQUEST_TIMEOUT*3)
        map_tar_ofile.close()
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'upload map_tar except:' + str(e)
        
    try:
        os.remove('./mapconf.tar')
    except Exception as e:
        return False, 'rm tar map except:' + str(e)
        
    return True, ''
    
def upload_other_file(user):
    svr_file = './excel/xls/MonsterAI.xml'
    upload_url = SERVER_URL + '/sifu_upload_' + user
        
    svr_file_handle = open(svr_file, 'rb+')
    files = {'file': svr_file_handle}
    try:
        r = requests.post(upload_url, files=files, timeout=REQUEST_TIMEOUT*3)
        svr_file_handle.close()
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'upload '+ svr_file + ' except:' + str(e)
        
    return True, ''
    
    

# 使用上传的配置，脚本，地图文件更新服务器
def update_server(user):
    update_url = SERVER_URL + '/sifu_update_' + user
    try:
        r = requests.get(update_url, timeout=REQUEST_TIMEOUT)
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'update_server except:' + str(e)

    return True, ''

# 停止服务器
def stop_server(user):
    stop_url = SERVER_URL + '/sifu_stop_' + user
    try:
        r = requests.get(stop_url, timeout=REQUEST_TIMEOUT)
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'stop_server except:' + str(e)

    return True, ''

# 重启服务器
def start_server(user):
    start_url = SERVER_URL + '/sifu_start_' + user
    try:
        r = requests.get(start_url, timeout=REQUEST_TIMEOUT)
        if r.text != 'ok':
            return False, r.text
    except Exception as e:
        return False, 'stop_server except:' + str(e)

    return True, ''

# 更新客户端本地配置
def update_client_bin(user):
    client_bin_path = './excel/data_out/client'
    dst_path = '../client/Assets/StreamingAssets/pc/text'
    try:
        for root, dirs, files in os.walk(client_bin_path):
            for file in files:
                dst_full_path = os.path.join(dst_path, file)
                if os.path.exists(dst_full_path):
                    os.remove(dst_full_path)
                shutil.copy(os.path.join(client_bin_path, file), dst_full_path)
    except Exception as e:
        return False, 'update_client_bin except:' + str(e)
    return True, ''

# 执行步骤
def execute_step(func, param, tip):
    print(tip + '...')
    ret, msg = func(param)
    if ret != True:
        print(tip + u', 执行失败, 错误信息:' + msg)
        print(u'私服更新失败')
        sys.exit(1)
    else:
        print(tip + u', 执行成功.')

# 命令行参数帮助信息
def usage():
    print("./sifu_update.py [zhaogang|heqiong|hejiawei|huayu]")
    sys.exit(1)

if __name__ == '__main__':
    if len(sys.argv) < 2:
        usage()

    name = sys.argv[1]
    if name != "zhaogang" and name != "heqiong" and name != "hejiawei" and name != "huayu" and name != "zhangjie" and name != "xiaolin" and name != "shengming" and name != "huzhen" :
        usage()

    execute_step(clear_upload_path, name, u'清空上传目录')
    execute_step(upload_bin_files, name, u'上传服务器配置文件')
    execute_step(upload_other_file, name, u'上传怪物AI')
    execute_step(upload_script_files, name, u'上传脚本配置文件')
    execute_step(upload_map_file, name, u'上传地图配置文件')
    execute_step(update_server, name, u'更新服务器私服配置')
    execute_step(stop_server, name, u'停止服务器')
    time.sleep(3)
    execute_step(start_server, name, u'启动服务器')
    execute_step(update_client_bin, name, u'更新客户端配置文件')
    print(u'私服更新成功')
    sys.exit(0)