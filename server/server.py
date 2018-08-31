# -*- coding: utf-8 -*-

import cv2
import numpy as np
from flask import Flask,request,helpers,make_response
from PIL import Image
import io

app = Flask("imageCovertServer")

@app.route("/convert/<int:id>", methods=["POST"])
def convert(id): 
    if request.headers["Content-Type"] == "application/octet-stream":
        print("step 1")
        print(id)
        data = request.data
        img = np.array(Image.open(io.BytesIO(data)))
        img = mosaic(img,0.05)
        # img = cv2.cvtColor(img,cv2.COLOR_RGB2GRAY)
        # img = cv2.Laplacian(img,cv2.CV_8U,ksize=3)
        # img = cv2.cvtColor(img,cv2.COLOR_GRAY2RGB)
        img = Image.fromarray(np.uint8(img))
        byteio = io.BytesIO()
        img.save(byteio,"png")
        response = make_response()
        response.data = byteio.getbuffer()
        response.headers["Content-type"] = "application/octet-stream"
        return response

def mosaic(img, alpha):
    # 画像の高さと幅
    w = img.shape[1]
    h = img.shape[0]

    # 縮小→拡大でモザイク加工
    img = cv2.resize(img,(int(w*alpha), int(h*alpha)))
    img = cv2.resize(img,(w, h), interpolation=cv2.INTER_NEAREST)

    return img

if __name__ == "__main__":
    app.run(host="0.0.0.0",port=5000)