type StaticInfo = {
  fontFamily: string;
};

export const useStatic = (): StaticInfo => ({
  fontFamily:
    "Iosevka Rx, Source Code Pro, -apple-system, BlinkMacSystemFont, Segoe UI," +
    "Roboto, Hiragino Sans GB, Microsoft YaHei UI, Microsoft YaHei, monospace, serif",
});
