
import ffmpeg
import numpy as np
import matplotlib.pyplot as plt
import time
# ffmpeg need to be instaled (avaliable from command line - added to system path enviromental variable)


def load_video(file_name):
    
    probe = ffmpeg.probe(file_name)
    video_stream = next((stream for stream in probe['streams'] if stream['codec_type'] == 'video'), None)
    width = int(video_stream['width'])
    height = int(video_stream['height'])
    
    fps = video_stream['r_frame_rate'].split('/')
    fps = float(fps[0]) / float(fps[1])
    
    
    if video_stream['pix_fmt'] == 'rgb24':
        fmt =  np.uint8
        color = 3
        pix_fmt_out = 'rgb24'
    elif video_stream['pix_fmt'] == 'bgr0':
        fmt =  np.uint8
        color = 3
        pix_fmt_out = 'rgb24'
    elif video_stream['pix_fmt'] == 'yuvj420p':
        fmt =  np.uint8
        color = 3
        pix_fmt_out = 'rgb24'
    elif 'gray16' in video_stream['pix_fmt']:
        fmt =  np.uint16
        color = 1
        pix_fmt_out = 'gray16'
    elif 'gray8':
        fmt =  np.uint8
        color = 1
        pix_fmt_out = 'gray8'
    else:
        raise('add this format condition ' + video_stream['pix_fmt'])
    
    
    
    out, _ = (
        ffmpeg
        .input(file_name)
        .output('pipe:', format='rawvideo', pix_fmt=pix_fmt_out)
        .run(capture_stdout=True)
        )
    
    

    data = np.frombuffer(out, fmt).reshape([-1, height, width, color])
    
    # data = data.reshape([-1, color, height, width]).transpose(0,2,3,1)

    return data, fps



def save_video(filename, data, fps, codec='ffv1'):
    
    
    # data shape is [frame, height, width, color]

    
    if len(data.shape) == 3:
        data = np.expand_dims(data, axis=3)
    
    
    if data.dtype == np.uint16 and data.shape[3] == 1:
        pix_fmt_in = 'gray16'
        pix_fmt_out = 'gray16le'
    elif data.dtype == np.uint8 and data.shape[3] == 3:
        pix_fmt_in = 'rgb24'
        pix_fmt_out = 'bgr0'
        # superted by ffv1 codec "ffmpeg -h encoder=ffv1 -v quiet" for list of avaliable and "ffmpeg -pix_fmts" says it has all bytes
    elif data.dtype == np.uint8 and data.shape[3] == 1:
        pix_fmt_in = 'gray8'
        pix_fmt_out = 'gray8'
    else:
        raise('format is not implemented')   
        

    
    
    ff_proc = (
        ffmpeg
        .input('pipe:',format='rawvideo',pixel_format=pix_fmt_in,s=str(data.shape[2]) + 'x' + str(data.shape[1]),r=str(fps))
        .output(filename,vcodec=codec,pix_fmt=pix_fmt_out, an=None)
        .overwrite_output()
        .run_async(pipe_stdin=True)
    )
    
    for frame_num in range(data.shape[0]):
    
        frame = data[frame_num, ...]
        
        ff_proc.stdin.write(frame)
    
    ff_proc.stdin.close()
    
    
    
if __name__ == '__main__':
    
    
    # start =  time.time()
    # data_16bit, fps = load_video('retina_gray16.avi')
    # save_video('retina_gray16_python.avi', data_16bit, fps)
    # data_16bit_new, fps = load_video('retina_gray16_python.avi')
    
    # print(np.sum(np.abs(data_16bit - data_16bit_new)))
    
    # print('time elapsed ' + str(time.time() - start))
    
    
    # start =  time.time()
    # data, fps = load_video('retina_rgb24.avi')
    # save_video('retina_rgb24_python.avi', data, fps)
    # data_new, fps = load_video('retina_rgb24_python.avi')
    
    # print(np.sum(np.abs(data - data_new)))
    
    # print('time elapsed ' + str(time.time() - start))
    
    
    start =  time.time()
    data_8bit, fps = load_video('retina_gray8.avi')
    save_video('retina_gray8_python.avi', data_8bit, fps)
    data_8bit_new, fps = load_video('retina_gray8_python.avi')
    
    print(np.sum(np.abs(data_8bit - data_8bit_new)))
    
    print('time elapsed ' + str(time.time() - start))
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    


