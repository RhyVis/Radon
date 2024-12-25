export type MdIndexDto = {
  path: string;
  name: string;
  desc: string;
  createTime: string;
  updateTime: string;
};

export type MdIndex = MdIndexDto & { content: string };

export type MdRecord = {
  name: string;
  desc: string;
  content: string;
};

export type MdUploadImageCallback = (
  files: Array<File>,
  callback: (urls: string[] | { url: string; alt: string; title: string }[]) => void,
) => void;
