import { MessagePlugin } from "tdesign-vue-next";

/**
 * Copy image from blob
 * @param image file blob
 */
const copyImage = async (image: Blob) => {
  try {
    await navigator.clipboard.write([
      new ClipboardItem({
        "image/png": image,
      }),
    ]);
    await MessagePlugin.success("复制图像成功");
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("复制图像失败");
  }
};

/**
 * Download image from blob
 * @param url file data url
 * @param fileName name of the file downloading
 */
const downloadImage = async (url: string, fileName: string) => {
  try {
    const link = document.createElement("a");
    link.download = fileName;
    link.href = url;
    link.click();
    await MessagePlugin.success("下载图像成功");
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("下载图像失败");
  }
};

export { copyImage, downloadImage };
