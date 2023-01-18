clc; clear all; close all;

% ffmpeg must be avaliable
ffmpeg_path = 'C:\Program Files\ffmpeg\bin\ffmpeg.exe';

tmpdir = './tmp';
mkdir(tmpdir)
vr = VideoReaderFFMPEG16bit('Gacr_01_001_01_580_m_short_16bit.avi','tempFolder', tmpdir, 'FFMPEGPath', ffmpeg_path);
vidframes = vr.read([1 vr.NumberOfFrames]);


width =size(vidframes,2);
height = size(vidframes,1);
n_frames = size(vidframes,4);
fps = vr.FrameRate;


v = FfmpegVideoWriter16bit('output.avi');
v.log_file = 'ffmpeg_log.txt';
v.cmd = [ffmpeg_path, ' -y -video_size ', num2str(width),'x', num2str(height), ...
         ' -pixel_format gray16 -f rawvideo -framerate ', num2str(fps), ...
         ' -i pipe:', ...
         ' -vcodec ffv1',...
         ' -pix_fmt gray16',...
         ' output.avi'];


open(v);

for i = 1:n_frames
    % Build synthetic image for testing.
    writeFrame(v, vidframes(:,:,1,i));

end
close(v)