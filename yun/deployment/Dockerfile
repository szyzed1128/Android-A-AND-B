# OBD云端Android模拟器镜像
# 基于docker-android项目

FROM budtmo/docker-android:emulator_11.0

LABEL maintainer="OBD Cloud Team"
LABEL description="OBD诊断应用云端化 - Android模拟器环境"

# 设置环境变量
ENV DEVICE="Samsung Galaxy S10"
ENV DATAPARTITION="4g"
ENV EMULATOR_ARGS="-memory 4096 -cores 4"

# 复制APK文件
COPY build_output/*.apk /app/obd-app.apk

# 复制启动脚本
COPY deployment/start_app.sh /app/start_app.sh
RUN chmod +x /app/start_app.sh

# 暴露端口
# 5555 - ADB
# 6080 - noVNC (可视化)
# 8080 - WebSocket
EXPOSE 5555 6080 8080

# 启动命令
CMD ["/app/start_app.sh"]
