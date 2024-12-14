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
    resp: string;
    tooltip: string;
    showResp: boolean;
  }[];
};

export interface PdxReplacerEntry {
  [key: string]: string;
}

export const enum PpStickyAction {
  REPLACE = "Replace",
  EVENT = "Event",
  EVENT_ID_PLUS = "Event+",
  EVENT_ID_MINUS = "Event-",
}
