export type PdxLangItem = {
  name: string;
  namespace: string[];
  value: string;
};

export type PdxLangEventItem = {
  name: string;
  desc: string;
  options: {
    key: string;
    name: string;
  }[];
};

export interface PdxReplacerEntry {
  [key: string]: string;
}
