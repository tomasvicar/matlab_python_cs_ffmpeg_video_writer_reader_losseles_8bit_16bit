clc;clear all;close all;
% tic
% [data_16bit,fps] = readFFFMPEGvideo('retina_gray16.avi', 'gray16', 2, 1, true);
% 
% writeFFFMPEGvideo('retina_gray16_matlab.avi', data_16bit, fps);
% 
% [data_16bit_new,fps] = readFFFMPEGvideo('retina_gray16_matlab.avi', 'gray16', 2, 1, true);
% 
% disp(sum(abs(data_16bit - data_16bit_new),'all'))
% toc


% tic
% [data,fps] = readFFFMPEGvideo('retina_rgb24.avi', 'rgb24', 3, 3, false);
% 
% writeFFFMPEGvideo('retina_rgb24_matlab.avi', data, fps);
% 
% [data_new,fps] = readFFFMPEGvideo('retina_rgb24_matlab.avi', 'rgb24', 3, 3, false);
% 
% disp(sum(abs(data- data_new),'all'))
% toc

tic
[data_8bit,fps] = readFFFMPEGvideo('retina_gray8.avi', 'gray8', 1, 1, false);

writeFFFMPEGvideo('retina_gray8_matlab.avi', data_8bit, fps);

[data_8bit_new,fps] = readFFFMPEGvideo('retina_gray8_matlab.avi', 'gray8', 1, 1, false);

disp(sum(abs(data_8bit - data_8bit_new),'all'))

toc



