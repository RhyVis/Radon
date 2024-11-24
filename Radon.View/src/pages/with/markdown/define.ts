import { apiDeleteNoContent, apiGet, apiPost, apiPut } from "@/lib/common/apiMethods.ts";

export type MdIndex = {
  path: string;
  name: string;
  desc: string;
};

export type MdRecord = {
  name: string;
  content: string;
};

export const getMdIndex = async (): Promise<MdIndex[]> => {
  const index = await apiGet<MdIndex[]>("/api/md/index");
  return index.data;
};

export const getMdRecord = async (path: string): Promise<MdRecord> => {
  const { data } = await apiGet<MdRecord>(`/api/md/${path}`);
  return data;
};

export const checkMdRecord = async (path: string): Promise<MdIndex> => {
  const { data } = await apiGet<MdIndex>(`/api/md/check/${path}`);
  return data;
};

export const updateMdRecord = async (path: string, name: string, desc: string, content: string): Promise<string> => {
  const res = await apiPut<string>(`api/md/${path}`, {
    path: path,
    name: name,
    desc: desc,
    content: content,
  });
  if (res.data.length > 0) console.warn("Path unexpectedly changed to", res.data);
  return res.data;
};

export const createMdRecord = async (name: string, desc: string, content: string): Promise<string> => {
  const res = await apiPost<string>(`api/md/create`, {
    path: "",
    name: name,
    desc: desc,
    content: content,
  });
  return res.data;
};

export const deleteMdRecord = async (path: string): Promise<void> => {
  await apiDeleteNoContent(`api/md/${path}`);
};
