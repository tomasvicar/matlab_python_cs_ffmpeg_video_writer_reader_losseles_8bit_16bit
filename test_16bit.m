clc; clear all; close all;


% ffmpeg must be avaliable
ffmpeg_path = 'C:\Program Files\ffmpeg\bin\ffmpeg.exe';

vidObj = VideoReader('Gacr_01_001_01_580_m_short.avi');
vidframes = read(vidObj);

vidframes = uint16(double(vidframes) * (2^16 / 2^8));
% vidframes


width = vidObj.Width;
height = vidObj.Height;
n_frames = vidObj.NumFrames;
fps = vidObj.FrameRate;

% v = FfmpegVideoWriter('output.avi');
% v.ffmpeg_cmd = ffmpeg_path;
% v.log_file = 'ffmpeg_log.txt';
% v.framerate = fps;
% v.pix_fmt = 'gray';
% v.vcodec = 'ffv1';



%%%% manual comand definition alternative
% v = FfmpegVideoWriter('output.avi');
v = FfmpegVideoWriter16bit('output.avi');
v.log_file = 'ffmpeg_log.txt';
v.cmd = [ffmpeg_path, ' -y -video_size ', num2str(width),'x', num2str(height), ...
         ' -pixel_format gray16 -f rawvideo -framerate ', num2str(fps), ...
         ' -i pipe:', ...
         ' -vcodec ffv1',...
         ' output.avi'];





open(v);

for i = 1:n_frames
    % Build synthetic image for testing.
    writeFrame(v, vidframes(:,:,1,i));

end
close(v)



% make sure it is looseles


% standard video reader is not working proprely (data are not exactly same) - why? (ffmpeg reader works ok)
% vidObj = VideoReader('output.avi');  
% vidframes_out = read(vidObj);


tmpdir = './tmp';
mkdir(tmpdir)
vr = VideoReaderFFMPEG16bit('output.avi','tempFolder', tmpdir, 'FFMPEGPath', ffmpeg_path);
vidframes_out = vr.read([1 vr.NumberOfFrames]);

% addpath('mmread')
% vidframes_out = mmread('output.avi',1:50);


disp(sum(abs(double(vidframes(:,:,1,:)) - double(vidframes_out(:,:,1,:))),'all'))





