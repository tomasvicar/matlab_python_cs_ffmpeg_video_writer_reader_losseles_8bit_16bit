clc;clear all;close all;

% [data_16bit_matlab,fps] = readFFFMPEGvideo('retina_gray16_matlab.avi', 'gray16', 2, 1, true);
% 
% [data_16bit_python,fps] = readFFFMPEGvideo('retina_gray16_python.avi', 'gray16', 2, 1, true);
% 
% [data_16bit_cs,fps] = readFFFMPEGvideo('retina_gray16_cs.avi', 'gray16', 2, 1, true);
% 
% disp(sum(abs(data_16bit_matlab - data_16bit_python) + abs(data_16bit_python - data_16bit_cs),'all'))



[data_matlab,fps] = readFFFMPEGvideo('retina_rgb24_matlab.avi', 'rgb24', 3, 3, false);

[data_python,fps] = readFFFMPEGvideo('retina_rgb24_python.avi', 'rgb24', 3, 3, false);

[data_cs,fps] = readFFFMPEGvideo('retina_rgb24_cs.avi', 'rgb24', 3, 3, false);

disp(sum(abs(data_matlab - data_python) + abs(data_python - data_cs),'all'))