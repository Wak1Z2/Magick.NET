#!/bin/bash
set -e

apt-get update

apt-get install ffmpeg fontconfig libgomp1 unzip -y

git clone https://github.com/ImageMagick/msttcorefonts msttcorefonts
cd msttcorefonts
. ./install.sh

fc-cache

wget https://github.com/ImageMagick/ImageMagick-Windows/releases/download/20200615/ghostscript-10.0.0-linux-x86_64.tgz
tar zxvf ghostscript-10.0.0-linux-x86_64.tgz
cp ghostscript-10.0.0-linux-x86_64/gs-1000-linux-x86_64 /usr/bin/gs
chmod 755 /usr/bin/gs
