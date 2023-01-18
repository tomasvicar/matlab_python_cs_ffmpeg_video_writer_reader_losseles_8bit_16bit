import imageio
import ffmpeg
import numpy as np
import matplotlib.pyplot as plt
# ffmpeg need to be instaled (avaliable from command line - added to system path enviromental variable)


def load_video_8bit(file_name):
    
    with imageio.get_reader(file_name) as f:
        imgs = []
        for frame_num, frame in enumerate(f):
            imgs.append(frame)
        return np.stack(imgs, axis=0)

def load_video(file_name):
    
    probe = ffmpeg.probe(file_name)
    video_stream = next((stream for stream in probe['streams'] if stream['codec_type'] == 'video'), None)
    width = int(video_stream['width'])
    height = int(video_stream['height'])
    
    
    out, _ = (
        ffmpeg
        .input(file_name)
        .output('pipe:', format='rawvideo', pix_fmt=video_stream['pix_fmt'])
        .run(capture_stdout=True)
        )
    
    if '16' in video_stream['pix_fmt']:
        fmt =  np.uint8
    else:
        fmt =  np.uint16
        
    if 'gray' in video_stream['pix_fmt']:
        color = 1
    elif 'rgb' in video_stream['pix_fmt']:
        color = 3
    else:
        raise('add this format condition')
        
    
    video = (
        np
        .frombuffer(out, np.uint16)
        .reshape([-1, height, width, color])
        )
    
    return video





def save_video(filename,output):
    
    frame_rate = 25 

    ff_proc = (
        ffmpeg
        .input('pipe:',format='rawvideo',pix_fmt='gray',s=str(output.shape[2]) + 'x' + str(output.shape[1]),r=str(frame_rate))
        .output(filename,vcodec='ffv1', an=None)
        .overwrite_output()
        .run_async(pipe_stdin=True)
    )
    
    for frame_num in range(output.shape[0]):
    
        frame = output[frame_num, :, :]
        
        frame = np.round(frame).astype(np.uint8)
        
        ff_proc.stdin.write(frame)
    
    ff_proc.stdin.close()


def save_video16bit(filename,output):
    
    frame_rate = 25 

    ff_proc = (
        ffmpeg
        .input('pipe:',format='rawvideo',pix_fmt='gray16',s=str(output.shape[2]) + 'x' + str(output.shape[1]),r=str(frame_rate))
        .output(filename,vcodec='ffv1', an=None)
        .overwrite_output()
        .run_async(pipe_stdin=True)
    )
    
    for frame_num in range(output.shape[0]):
    
        frame = output[frame_num, :, :]
        
        frame = np.round(frame).astype(np.uint16)
        
        ff_proc.stdin.write(frame)
    
    ff_proc.stdin.close()




data = load_video_8bit('Gacr_01_001_01_580_m_short.avi')
data = (data.astype(np.float64) * (2**16 / 2**8)).astype(np.uint16)


save_video16bit('output.avi',data[:,:,:,0])



data_out = load_video('output.avi')
# plt.imshow(data_out[0,:,:,0])


print(np.sum(np.abs(data[:,:,:,0] - data_out[:,:,:,0])))