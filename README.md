# Playtime 2025 

`ssh root@icefish.local`
`/persistent/gaute`
`/bin/advmuxctl rxport 2`

### On my mac:

`ffmpeg -i hedda.jpg -vf scale=1920:1080 -f rawvideo -pix_fmt rgba framebuffer1080.raw`

`scp -P 22 ~/Downloads/framebuffer1080.raw root@icefish.local:/persistent/gaute/`

### On c1:

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
## Install npm 
tar -xvf node-v23.10.0-linux-arm64.tar.xz  
scp -r ./node-v23.10.0-linux-arm64  root@icefish.local:/

### On c1 
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
