# Showing image on c1 🖼️

`ssh root@icefish.local`
`/persistent/gaute`
`/bin/advmuxctl rxport 2`

## On my mac:

`ffmpeg -i hedda.jpg -vf scale=1920:1080 -f rawvideo -pix_fmt rgba framebuffer1080.raw`

`scp -P 22 ~/Downloads/framebuffer1080.raw root@icefish.local:/persistent/gaute/`

## On c1:

draw_image.sg
```
IMAGE="./framebuffer1080.raw"
START_ROW=0
IMG_WIDTH=1920
IMG_HEIGHT=1080
FB_WIDTH=4096
BPP=4

for row in $(seq 0 $((IMG_HEIGHT - 1))); do
  img_offset=$((row * IMG_WIDTH * BPP))
  fb_offset=$(((START_ROW + row) * FB_WIDTH * BPP))
  dd if="$IMAGE" of=/dev/fb0 bs=1 skip=$img_offset seek=$fb_offset count=$((IMG_WIDTH * BPP)) status=none
done
```
#### Install npm 
tar -xvf node-v23.10.0-linux-arm64.tar.xz  
scp -r ./node-v23.10.0-linux-arm64  root@icefish.local:/

#### On c1 
mv /node-v23.10.0-linux-arm64 /opt/nodejs
 ln -s /opt/nodejs/bin/node /usr/bin/node
 ln -s /opt/nodejs/bin/npm /usr/bin/npm

```
cat << 'EOF' > /usr/bin/npm
#!/bin/sh
exec node /opt/nodejs/lib/node_modules/npm/bin/npm-cli.js "$@"
EOF
chmod +x /usr/bin/npm
```
# 🧪 Raspberry Pi OS Lite – Chromium Kiosk via Wayland (Weston)

This guide shows how to run **Chromium in kiosk mode** on **Raspberry Pi OS Lite** using **Weston** (Wayland compositor), outputting via **HDMI** — no X11, no desktop environment required.

---

## 🚀 What You’ll Get

- Weston running directly on DRM/KMS
- Chromium in fullscreen kiosk mode
- Local HTML/JS app served from file
- Screen stays awake, no idle blanking
- All lightweight — perfect for embedded systems

---

## 📦 Install Required Packages

```bash
sudo apt update
sudo apt install \
  weston \
  chromium \
  fonts-dejavu-core \
  libinput-tools
```

Optional (for debugging):

```bash
sudo apt install \
  mesa-utils \
  xkb-data \
  fontconfig \
  curl unzip
```

---

## 🗂️ Setup Weston Runtime Directory

```bash
sudo mkdir -p /run/user/0
sudo chmod 700 /run/user/0
```

---

## 💡 Prevent HDMI Sleep

### `/boot/firmware/config.txt`

```bash
sudo nano /boot/firmware/config.txt
```

Add at the bottom:

```ini
# Prevent HDMI from sleeping
consoleblank=0
hdmi_blanking=1
```

### `/boot/firmware/cmdline.txt`

```bash
sudo nano /boot/firmware/cmdline.txt
```

Append to the **end of the single line** (don’t press Enter):

```
consoleblank=0
```

Then:

```bash
sudo reboot
```

---

## 🖥️ Launch Weston (on HDMI)

```bash
sudo -E env XDG_RUNTIME_DIR=/run/user/0 weston \
  --tty=1 \
  --backend=drm-backend.so \
  --log=/tmp/weston.log
```

Leave this running. Your HDMI screen should turn gray.

---

## 🔓 Allow Chromium to Access Wayland Socket

In another terminal:

```bash
sudo chmod 666 /run/user/0/wayland-1
```

> Adjust to match the actual socket Weston created (e.g. `wayland-0`, `wayland-1`)

---

## 🌐 Launch Chromium in Kiosk Mode

```bash
sudo WAYLAND_DISPLAY=wayland-1 XDG_RUNTIME_DIR=/run/user/0 chromium \
  --ozone-platform=wayland \
  --enable-features=UseOzonePlatform \
  --kiosk \
  --no-sandbox \
  --no-first-run \
  --disable-translate \
  --disable-infobars \
  --allow-file-access-from-files \
  file:///home/gaute/playtime-spring-2025/dist/index.html
```

---

## 💤 (Optional) Disable Weston Idle Timeout

```bash
mkdir -p ~/.config
nano ~/.config/weston.ini
```

```ini
[core]
idle-time=0
```

---

## ✅ Done!

You now have a headless Raspberry Pi kiosk setup running:

- ✅ Wayland (Weston)
- ✅ Chromium in kiosk mode
- ✅ Local app output on HDMI
- ✅ No desktop, minimal resources

Perfect for signage, embedded dashboards, or boot-to-app devices.
