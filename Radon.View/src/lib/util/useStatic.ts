type StaticInfo = {
  fontFamily: string;
};

const staticInfo: StaticInfo = {
  fontFamily:
    "Iosevka Rx, Source Code Pro, -apple-system, BlinkMacSystemFont, Segoe UI," +
    "Roboto, Hiragino Sans GB, Microsoft YaHei UI, Microsoft YaHei, monospace, serif",
};

/** Static info object */
export default (): StaticInfo => staticInfo;
