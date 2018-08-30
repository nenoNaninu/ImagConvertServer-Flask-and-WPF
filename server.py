# -*- coding: utf-8 -*-

import cv2
import numpy as np
from flask import Flask,request,helpers,make_response
from PIL import Image
import io

app = Flask("imageCovertServer")

@app.route("/convert", methods=["POST"])
def convert(): 
    if request.headers["Content-Type"] == "application/octet-stream":
        print("step 1")
        data = request.data
        img = np.array(Image.open(io.BytesIO(data)))
        img = cv2.cvtColor(img,cv2.COLOR_RGB2GRAY)
        img = cv2.Laplacian(img,cv2.CV_32F,ksize=3)
        img = cv2.cvtColor(img,cv2.COLOR_GRAY2RGB)
        img = Image.fromarray(np.uint8(img))
        byteio = io.BytesIO()
        img.save(byteio,"bmp")
        response = make_response()
        response.data = byteio.getbuffer()
        response.headers["Content-type"] = "application/octet-stream"
        return response

if __name__ == "__main__":
    app.run()

