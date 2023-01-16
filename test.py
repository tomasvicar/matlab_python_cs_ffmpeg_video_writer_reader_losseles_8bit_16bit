import imageio
import ffmpeg
import numpy as np
import matplotlib.pyplot as plt
# ffmpeg need to be instaled (avaliable from command line - added to system path enviromental variable)


def load_video(file_name):
    
    with imageio.get_reader(file_name) as f:
        imgs = []
        for frame_num, frame in enumerate(f):
            imgs.append(frame)
        return np.stack(imgs, axis=0)
    


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





# data = load_video('Gacr_01_001_01_580_m_short.avi')



# save_video('output.avi',data[:,:,:,0])



data_out = load_video('output.avi')
plt.imshow(data_out[0,:,:,0])


# print(np.sum(np.abs(data[:,:,:,0] - data_out[:,:,:,0])))