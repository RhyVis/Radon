export type SaveBrief = {
  id: number;
  note: string;
};

export type SaveEntry = SaveBrief & {
  text: string;
};
