import axiosInstance from "@/lib/common/apiHttp";
import axios from "axios";

type DownloadSegment = {
  url: string;
  start: number;
  end: number;
};

const downloadSegment = async (segment: DownloadSegment): Promise<ArrayBuffer> => {
  const response = await axiosInstance.get(segment.url, {
    responseType: "arraybuffer",
    headers: {
      Range: `bytes=${segment.start}-${segment.end}`,
    },
    timeout: 0,
  });
  return response.data;
};

const downloadChunked = async (url: string, segmentSize: number): Promise<ArrayBuffer> => {
  const response = await axiosInstance.head(url, { timeout: 0 });
  const fileSize = parseInt(response.headers["content-length"], 10);
  const segments: DownloadSegment[] = [];

  for (let start = 0; start < fileSize; start += segmentSize) {
    const end = Math.min(start + segmentSize - 1, fileSize - 1);
    segments.push({ url, start, end });
  }

  const segmentPromises = segments.map(downloadSegment);
  const segmentData = await axios.all(segmentPromises);

  const fileData = new Uint8Array(fileSize);
  let offset = 0;
  for (const data of segmentData) {
    fileData.set(new Uint8Array(data), offset);
    offset += data.byteLength;
  }

  return fileData.buffer;
};

export { downloadChunked };
