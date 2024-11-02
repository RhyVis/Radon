type Card = {
  index: number;
  name: string;
  loc: string;
  img: string;
  rev: boolean;
  desc: {
    upright: string;
    reverse: string;
    desc: string[];
  };
};

type CardDisplay = {
  data: Card;
  showImg: boolean;
  showDesc: boolean;
};

type DeckInfo = {
  name: string;
  loc: string;
  full: boolean;
};

interface DeckInfoInterface {
  [name: string]: DeckInfo;
}

type DeckInfoSelect = {
  label: string;
  value: string;
};

export type { Card, CardDisplay, DeckInfo, DeckInfoInterface, DeckInfoSelect };
