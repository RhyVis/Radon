/**
 * Transform b64 data to blob
 * @param b64Data b64 string
 * @param contentType data type to covert to
 * @param sliceSize slice data size
 */
const b64ToBlob = (b64Data: string, contentType?: string, sliceSize?: number) => {
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

export { b64ToBlob };
