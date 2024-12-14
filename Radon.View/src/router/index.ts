import HomeView from "@/pages/base/home/index.vue";
import { authGuard, onlineGuard } from "@/router/guard";
import {
  baseRecords,
  dataRecords,
  drawRecords,
  markdownRecords,
  mathRecords,
  mystRecords,
  pdxParserRecords,
  utilRecords,
} from "@/router/records";
import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";

const records: RouteRecordRaw[] = [
  // Homepage
  {
    path: "/",
    name: "Home",
    component: HomeView,
    meta: {
      title: "Radon",
    },
  },

  // Complex
  ...markdownRecords,
  ...pdxParserRecords,

  // Simple
  ...baseRecords,
  ...dataRecords,
  ...mystRecords,
  ...drawRecords,
  ...mathRecords,
  ...utilRecords,

  // Err Handle
  {
    path: "/error",
    name: "404",
    component: () => import("@/pages/base/error/index.vue"),
    meta: {
      title: "错误",
    },
  },
  {
    path: "/:pathMatch(.*)",
    name: "404",
    component: () => import("@/pages/base/error/index.vue"),
    meta: {
      title: "错误",
    },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes: records,
});

router.beforeEach(async to => {
  if (await onlineGuard(to)) {
    return await authGuard(to);
  } else {
    return false;
  }
});

export default router;
