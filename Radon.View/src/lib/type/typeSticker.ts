type CharacterDefinition = {
  id: number;
  name: string;
  chara: string;
  img: string;
  color: string;
  defaultText: {
    text: string;
    x: number;
    y: number;
    r: number;
    s: number;
  };
};

type DrawConf = {
  charaID: number;
  fontSize: number;
  spaceSize: number;
  rotate: number;
  x: number;
  y: number;
  text: string;
  curve: boolean;
  useCommercialFonts: boolean;
};

export type { CharacterDefinition, DrawConf };
