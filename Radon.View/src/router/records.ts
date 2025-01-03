import "vue-router";
import type { RouteRecordRaw } from "vue-router";

declare module "vue-router" {
  interface RouteMeta {
    title: string;
    tKey?: string;
    icon?: string;
    auth?: boolean;
    online?: boolean;
  }
}

export const baseRecords: RouteRecordRaw[] = [
  {
    path: "/credits",
    name: "Credits",
    component: () => import("@/pages/base/credits/index.vue"),
    meta: {
      title: "Credits",
      tKey: "route.credits",
    },
  },
  {
    path: "/auth",
    name: "Auth",
    component: () => import("@/pages/base/auth/index.vue"),
    meta: {
      title: "Auth",
      tKey: "route.auth",
      online: true,
    },
  },
];

export const dataRecords: RouteRecordRaw[] = [
  {
    path: "/spam",
    name: "Spam",
    component: () => import("@/pages/data/spam/index.vue"),
    meta: {
      title: "弹药库",
      icon: "loudspeaker",
      tKey: "route.data.spam",
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
      tKey: "route.data.save",
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
      tKey: "route.data.nav",
      auth: true,
      online: true,
    },
  },
];

export const mystRecords: RouteRecordRaw[] = [
  {
    path: "/tarot",
    name: "Tarot",
    component: () => import("@/pages/myst/tarot/index.vue"),
    meta: {
      title: "电子塔罗",
      icon: "card",
      tKey: "route.myst.tarot",
      online: true,
    },
  },
];

export const mathRecords: RouteRecordRaw[] = [
  {
    path: "/radix",
    name: "Radix",
    component: () => import("@/pages/math/radix/index.vue"),
    meta: {
      title: "进制转换",
      icon: "calculation-1",
      tKey: "route.math.radix",
    },
  },
  {
    path: "/roman",
    name: "RomanNumeral",
    component: () => import("@/pages/math/roman/index.vue"),
    meta: {
      title: "罗马数字",
      icon: "castle",
      tKey: "route.math.roman",
    },
  },
];

export const drawRecords: RouteRecordRaw[] = [
  {
    path: "/pjsk-sticker",
    name: "PJSK-Sticker",
    component: () => import("@/pages/draw/pjsk-sticker/index.vue"),
    meta: {
      title: "PJSK表情",
      icon: "edit-2",
      tKey: "route.draw.pjskSticker",
      online: true,
    },
  },
  {
    path: "/arcaea-sticker",
    name: "Arcaea-Sticker",
    component: () => import("@/pages/draw/arcaea-sticker/index.vue"),
    meta: {
      title: "Arcaea表情",
      icon: "edit-2",
      tKey: "route.draw.arcaeaSticker",
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
      tKey: "route.draw.baBanner",
      online: true,
    },
  },
];

export const utilRecords: RouteRecordRaw[] = [
  {
    path: "/codex",
    name: "Codex",
    component: () => import("@/pages/util/codex/index.vue"),
    meta: {
      title: "抽象转义",
      icon: "chat-error",
      tKey: "route.util.codex",
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
      tKey: "route.util.reverse",
    },
  },
  {
    path: "/base64",
    name: "Base64",
    component: () => import("@/pages/util/base64/index.vue"),
    meta: {
      title: "Base64",
      icon: "file-code-1",
      tKey: "route.util.base64",
    },
  },
  {
    path: "/you-are-right",
    name: "You Are Right",
    component: () => import("@/pages/util/you-are-right/index.vue"),
    meta: {
      title: "You Are Right",
      icon: "filter-2",
      tKey: "route.util.youAreRight",
    },
  },
];

export const markdownRecords: RouteRecordRaw[] = [
  // Menu
  {
    path: "/archive",
    children: [
      {
        // Index
        path: "",
        name: "Archive",
        component: () => import("@/pages/with/archive/index.vue"),
        meta: {
          title: "Archive",
          online: true,
        },
      },
      {
        // Create
        path: "create",
        name: "MarkdownCreate",
        component: () => import("@/pages/with/archive/create/index.vue"),
        meta: {
          title: "Markdown Creator",
          online: true,
        },
      },
      {
        path: ":p",
        children: [
          {
            // Read
            path: "read",
            name: "MarkdownReader",
            component: () => import("@/pages/with/archive/read/index.vue"),
            meta: {
              title: "Markdown Reader",
              online: true,
            },
          },
          {
            // Edit
            path: "edit",
            name: "MarkdownEditor",
            component: () => import("@/pages/with/archive/edit/index.vue"),
            meta: {
              title: "Markdown Editor",
              online: true,
            },
          },
        ],
      },
    ],
  },
];

export const pdxParserRecords: RouteRecordRaw[] = [
  {
    path: "/pdx-parser",
    children: [
      {
        path: "",
        name: "PdxParser",
        component: () => import("@/pages/with/pdx-parser/index.vue"),
        meta: {
          title: "PDX Parser",
          online: true,
        },
      },
      {
        path: "lang-tree",
        name: "LangTree",
        component: () => import("@/pages/with/pdx-parser/lang-tree/index.vue"),
        meta: {
          title: "Lang Tree",
          online: true,
        },
      },
      {
        path: "lang-event",
        name: "LangEvent",
        component: () => import("@/pages/with/pdx-parser/lang-event/index.vue"),
        meta: {
          title: "Lang Event",
          online: true,
        },
      },
    ],
  },
];
