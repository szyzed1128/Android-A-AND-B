import os
import shutil
import subprocess
import sys
import glob

# ================= 配置区域 =================

# 1. 工具路径配置
CMD_APKTOOL = ["java", "-jar", r"C:\apktool\apktool.jar"]
CMD_ZIPALIGN = r"C:\Users\szyze\AppData\Local\Android\Sdk\build-tools\36.1.0\zipalign.exe"
CMD_APKSIGNER = r"C:\Users\szyze\AppData\Local\Android\Sdk\build-tools\36.1.0\apksigner.bat"
CMD_KEYTOOL = "keytool"

# 2. 签名配置
KEYSTORE_PATH = "release.keystore" 
KEY_ALIAS = "my_alias"
KEY_STORE_PASS = "123456"
KEY_PASS = "123456"

# ===========================================

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
INPUT_DIR = os.path.join(BASE_DIR, "original_apk")
SOURCE_DIR = os.path.join(BASE_DIR, "my_source_code")
OUTPUT_DIR = os.path.join(BASE_DIR, "build_output")
TEMP_WORK_DIR = os.path.join(BASE_DIR, "temp_decompiled")

def log(msg):
    print(f"\n[+] {msg}")

def error(msg):
    print(f"\n[!] 错误: {msg}")
    sys.exit(1)

def run_cmd(cmd):
    """通用命令运行函数，处理列表和字符串"""
    try:
        # 如果是列表且第一个元素也是列表，平铺它
        if isinstance(cmd, list) and len(cmd) > 0 and isinstance(cmd[0], list):
            flat_cmd = []
            for item in cmd:
                if isinstance(item, list): flat_cmd.extend(item)
                else: flat_cmd.append(item)
            cmd = flat_cmd
            
        # 统一使用列表调用，关闭 shell 以避免交互挂起
        subprocess.check_call(cmd, shell=False)
    except subprocess.CalledProcessError as e:
        error(f"命令执行失败: {e}")
    except Exception as e:
        error(f"发生异常: {e}")

def check_tools():
    log("正在检查环境工具...")
    # 仅作简单路径检查提示
    pass

def clean_temp():
    if os.path.exists(TEMP_WORK_DIR):
        log(f"清理临时目录: {TEMP_WORK_DIR}")
        shutil.rmtree(TEMP_WORK_DIR)

def find_apk():
    apks = glob.glob(os.path.join(INPUT_DIR, "*.apk"))
    if not apks:
        error(f"在 {INPUT_DIR} 下未找到任何 .apk 文件。")
    return apks[0]

def ensure_keystore():
    ks_path = os.path.join(BASE_DIR, KEYSTORE_PATH)
    if not os.path.exists(ks_path):
        log("未检测到签名密钥，正在生成测试密钥...")
        cmd = [
            CMD_KEYTOOL, "-genkey", "-v", 
            "-keystore", ks_path, 
            "-alias", KEY_ALIAS, 
            "-keyalg", "RSA", 
            "-keysize", "2048", 
            "-validity", "10000",
            "-storepass", KEY_STORE_PASS,
            "-keypass", KEY_PASS,
            "-dname", "CN=MyCustomUI, OU=Dev, O=MyCompany, L=City, S=State, C=US"
        ]
        run_cmd(cmd)
    return ks_path

def main():
    print("=========================================")
    print("      Android APK UI 替换自动化工厂      ")
    print("=========================================")
    
    apk_path = find_apk()
    apk_name = os.path.basename(apk_path)
    log(f"找到原始 APK: {apk_name}")

    clean_temp()

    # 3. 反编译
    log("正在反编译 APK...")
    # 组合命令列表: java -jar ... d apk -o dir -f
    cmd_decode = CMD_APKTOOL + ["d", apk_path, "-o", TEMP_WORK_DIR, "-f"]
    run_cmd(cmd_decode)

    # 4. 替换 UI 文件
    log("正在注入新的 UI 源码...")
    # 修正目标路径为 assets/dist
    assets_dir = os.path.join(TEMP_WORK_DIR, "assets", "dist")
    
    if os.path.exists(assets_dir):
        # 如果存在，先清空旧的 dist 内容，确保干净替换
        shutil.rmtree(assets_dir)
    os.makedirs(assets_dir)
    
    for item in os.listdir(SOURCE_DIR):
        s = os.path.join(SOURCE_DIR, item)
        d = os.path.join(assets_dir, item)
        if os.path.isdir(s):
            # 如果是文件夹 (如 assets)，递归复制
            if os.path.exists(d): shutil.rmtree(d)
            shutil.copytree(s, d)
        else:
            # 如果是文件 (如 index.html)
            shutil.copy2(s, d)

    # 4.5 复制修改后的 DLL 文件（如果存在）
    modified_dll = os.path.join(BASE_DIR, "temp_inspect", "unknown", "assemblies", "CarDemo.dll")
    log(f"检查修改后的 DLL: {modified_dll}")
    log(f"DLL 文件存在: {os.path.exists(modified_dll)}")
    if os.path.exists(modified_dll):
        log("正在注入修改后的 CarDemo.dll...")
        target_dll = os.path.join(TEMP_WORK_DIR, "unknown", "assemblies", "CarDemo.dll")
        log(f"目标路径: {target_dll}")
        target_dir = os.path.dirname(target_dll)
        if not os.path.exists(target_dir):
            log(f"目标目录不存在，创建: {target_dir}")
            os.makedirs(target_dir)
        shutil.copy2(modified_dll, target_dll)
        log("DLL 注入完成")

    # 5. 回编译
    log("正在重新编译 APK...")
    unsigned_apk = os.path.join(OUTPUT_DIR, "unsigned.apk")
    if not os.path.exists(OUTPUT_DIR): os.makedirs(OUTPUT_DIR)
    
    cmd_build = CMD_APKTOOL + ["b", TEMP_WORK_DIR, "-o", unsigned_apk]
    run_cmd(cmd_build)

    # 6. 对齐
    log("正在进行内存对齐...")
    aligned_apk = os.path.join(OUTPUT_DIR, "aligned.apk")
    if os.path.exists(aligned_apk): os.remove(aligned_apk)
    cmd_align = [CMD_ZIPALIGN, "-p", "-f", "-v", "4", unsigned_apk, aligned_apk]
    run_cmd(cmd_align)

    # 7. 签名
    log("正在签名 APK...")
    final_apk = os.path.join(OUTPUT_DIR, f"Release_{apk_name}")
    ks_path = ensure_keystore()
    cmd_sign = [
        CMD_APKSIGNER, "sign",
        "--ks", ks_path,
        "--ks-pass", f"pass:{KEY_STORE_PASS}",
        "--out", final_apk,
        aligned_apk
    ]
    run_cmd(cmd_sign)

    # 清理中间产物
    if os.path.exists(unsigned_apk): os.remove(unsigned_apk)
    if os.path.exists(aligned_apk): os.remove(aligned_apk)
    clean_temp()

    print(f"\n[OK] 成功! 新 APK: {final_apk}")

if __name__ == "__main__":
    main()