import type { RouteRecordRaw } from "vue-router";

type RouteRecordMeta = {
  title: string;
  icon?: string;
  auth?: boolean;
  online?: boolean;
};
type RouteRecordAssemble = RouteRecordRaw & {
  meta: RouteRecordMeta;
};

const baseRecords: RouteRecordAssemble[] = [
  {
    path: "/credits",
    name: "Credits",
    component: () => import("@/pages/base/credits/index.vue"),
    meta: {
      title: "Credits",
    },
  },
  {
    path: "/auth",
    name: "Auth",
    component: () => import("@/pages/base/auth/index.vue"),
    meta: {
      title: "Auth",
      online: true,
    },
  },
];

const dataRecords: RouteRecordAssemble[] = [
  {
    path: "/spam",
    name: "Spam",
    component: () => import("@/pages/data/spam/index.vue"),
    meta: {
      title: "弹药库",
      icon: "loudspeaker",
      online: true,
    },
  },
  {
    path: "/save",
    name: "Save",
    component: () => import("@/pages/data/save/index.vue"),
    meta: {
      title: "字符存储",
      icon: "save",
      auth: true,
      online: true,
    },
  },
  {
    path: "/nav",
    name: "Nav",
    component: () => import("@/pages/data/nav/index.vue"),
    meta: {
      title: "导航",
      icon: "indicator",
      auth: true,
      online: true,
    },
  },
];

const mystRecords: RouteRecordAssemble[] = [
  {
    path: "/tarot",
    name: "Tarot",
    component: () => import("@/pages/myst/tarot/index.vue"),
    meta: {
      title: "电子塔罗",
      icon: "card",
      online: true,
    },
  },
];

const mathRecords: RouteRecordAssemble[] = [
  {
    path: "/radix",
    name: "Radix",
    component: () => import("@/pages/math/radix/index.vue"),
    meta: {
      title: "进制转换",
      icon: "calculation-1",
    },
  },
  {
    path: "/roman",
    name: "RomanNumeral",
    component: () => import("@/pages/math/roman/index.vue"),
    meta: {
      title: "罗马数字",
      icon: "castle",
    },
  },
];

const drawRecords: RouteRecordAssemble[] = [
  {
    path: "/pjsk-sticker",
    name: "PJSK-Sticker",
    component: () => import("@/pages/draw/pjsk-sticker/index.vue"),
    meta: {
      title: "PJSK表情",
      icon: "edit-2",
      online: true,
    },
  },
  {
    path: "/ba-banner",
    name: "BA-Banner",
    component: () => import("@/pages/draw/ba-banner/index.vue"),
    meta: {
      title: "BA横幅",
      icon: "typography",
      online: true,
    },
  },
];

const utilRecords: RouteRecordAssemble[] = [
  {
    path: "/codex",
    name: "Codex",
    component: () => import("@/pages/util/codex/index.vue"),
    meta: {
      title: "抽象转义",
      icon: "chat-error",
      online: true,
    },
  },
  {
    path: "/reverse",
    name: "Reverse",
    component: () => import("@/pages/util/reverse/index.vue"),
    meta: {
      title: "反转机",
      icon: "refresh",
    },
  },
  {
    path: "/base64",
    name: "Base64",
    component: () => import("@/pages/util/base64/index.vue"),
    meta: {
      title: "Base64",
      icon: "file-code-1",
    },
  },
  {
    path: "/pdx",
    name: "PDX Parser",
    component: () => import("@/pages/util/pdx-parser/index.vue"),
    meta: {
      title: "PDX Parser",
      icon: "ruler",
      online: true,
    },
  },
];

export { baseRecords, dataRecords, drawRecords, mathRecords, mystRecords, utilRecords };
export type { RouteRecordAssemble };

export const markdownRecords: RouteRecordAssemble[] = [
  // Menu
  {
    path: "/md",
    name: "Markdown",
    component: () => import("@/pages/with/markdown/index.vue"),
    meta: {
      title: "Markdown",
    },
  },
  // Create
  {
    path: "/md/create",
    name: "MarkdownCreate",
    component: () => import("@/pages/with/markdown/create/index.vue"),
    meta: {
      title: "Markdown Creator",
    },
  },
  // Read
  {
    path: "/md/:p/read",
    name: "MarkdownReader",
    component: () => import("@/pages/with/markdown/read/index.vue"),
    meta: {
      title: "Markdown Reader",
    },
  },
  // Write
  {
    path: "/md/:p/write",
    name: "MarkdownWriter",
    component: () => import("@/pages/with/markdown/write/index.vue"),
    meta: {
      title: "Markdown Writer",
    },
  },
];
