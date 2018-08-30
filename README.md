# ImagConvertServer-Flask-and-WPF
wpf(C#)からFlask(python)に画像を投げて、サーバーに画像変換させてC#側に返すプログラム一式

```
cd server
python server.py
```
でサーバー立ち上げて、c#のwpfを開いて(バイナリは突っ込んでないのでビルドしてください)画像選択、アップロードすると変換されて返ってくる。

例外とかエラー処理とか書いてないので画像以外送信するとぽしゃる。

bytesとPILとndarrayの変換が大事。
