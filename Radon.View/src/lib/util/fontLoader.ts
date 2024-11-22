import type { Loader } from "@/composable/useLoader";

type FontInfo = {
  name: string;
  url: string;
};

const fontList: FontInfo[] = [
  {
    name: "YurukaStd",
    url: "/font/YurukaStd.woff2",
  },
  {
    name: "SSFangTangTi",
    url: "/font/ShangShouFangTangTi.woff2",
  },
  {
    name: "LilitaOne",
    url: "/font/LilitaOne-Regular.ttf",
  },
  {
    name: "ChildFunSansFusion-Z",
    url: "/font/ChildFunSansFusion-Z.ttf",
  },
  {
    name: "RoGSanSrfStd-Bd",
    url: "/font/RoGSanSrfStd-Bd_other_mod.woff2",
  },
  {
    name: "RoGSanSrfStd-Bd",
    url: "/font/RoGSanSrfStd-Bd_CJK.woff2",
  },
  {
    name: "GlowSans",
    url: "/font/GlowSansSC-Normal-Heavy.otf",
  },
  {
    name: "IosevkaRx",
    url: "/font/IosevkaRx-Regular.woff2",
  },
  {
    name: "IosevkaRx",
    url: "/font/IosevkaRx-Bold.woff2",
  },
];

const base = import.meta.env.VITE_RES_ROOT;
const url = (s: string): string => `url("${base + s}")`;

const getFontLoaders = (): Loader[] => {
  return fontList.map(font => {
    return {
      name: font.name,
      action: new Promise<void>(async (resolve, reject) => {
        try {
          if (typeof FontFace !== "undefined") {
            const fontFace = new FontFace(font.name, url(font.url));
            const loadedFont = await fontFace.load();
            document.fonts.add(loadedFont);
            console.debug(`Successfully loaded ${font.name}`);
          } else {
            const style = document.createElement("style");
            style.textContent = `
              @font-face {
                font-family: "${font.name}";
                src: ${url(font.url)};
              }
            `;
            document.head.appendChild(style);
            console.debug(`Successfully loaded ${font.name}`);
          }
          resolve();
        } catch (e) {
          console.error(`Failed to load ${font.name}`);
          reject(e);
        }
      }),
    };
  });
};

export { getFontLoaders };
