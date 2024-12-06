import i18n from "@/locale";
import { MessagePlugin } from "tdesign-vue-next";

const { t } = i18n.global;

export const b64ToBlob = (b64Data: string, contentType?: string, sliceSize?: number) => {
  contentType = contentType || "image/png";
  sliceSize = sliceSize || 512;
  const byteCharacters = atob(b64Data);
  const byteArrays = [];
  for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
    const slice = byteCharacters.slice(offset, offset + sliceSize);
    const byteNumbers = new Array(slice.length);
    for (let i = 0; i < slice.length; i++) {
      byteNumbers[i] = slice.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    byteArrays.push(byteArray);
  }
  return new Blob(byteArrays, { type: contentType });
};

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
    await MessagePlugin.success(t("message.image.copySuccess"));
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("message.image.copyFail"));
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
    await MessagePlugin.success(t("message.image.downloadSuccess"));
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("message.image.downloadFail"));
  }
};

export { copyImage, downloadImage };
