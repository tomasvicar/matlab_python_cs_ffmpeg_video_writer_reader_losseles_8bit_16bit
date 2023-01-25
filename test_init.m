clc;clear all;close all;
tic
[data_8bit,fps] = readFFFMPEGvideo('retina_rgb24.avi', 'rgb24', 3, 3, false);
data_8bit = data_8bit(:,:,1,:);

data_16bit = double(data_8bit) * 255;
noise = 255 * randn(size(data_16bit)) .* (rand(size(data_16bit)) > 0.03);
data_16bit = uint16(data_16bit + noise);


writeFFFMPEGvideo('retina_gray16.avi', data_16bit, fps);

[data_16bit_load,fps] = readFFFMPEGvideo('retina_gray16.avi', 'gray16', 2, 1, true);

disp(sum(abs(data_16bit_load - data_16bit),'all'))
toc



writeFFFMPEGvideo('retina_gray8.avi', data_8bit, fps);

[data_8bit_load,fps] = readFFFMPEGvideo('retina_gray8.avi', 'gray8', 1, 1, false);

disp(sum(abs(data_8bit_load - data_8bit),'all'))
toc

