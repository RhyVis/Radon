import HomeView from "@/pages/base/home/index.vue";
import { authGuard, onlineGuard } from "@/router/guard";
import type { RouteRecordAssemble } from "@/router/records";
import { baseRecords, dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { createRouter, createWebHistory } from "vue-router";

const records: RouteRecordAssemble[] = [
  {
    path: "/",
    name: "Home",
    component: HomeView,
    meta: {
      title: "Radon",
    },
  },
  ...baseRecords,
  ...dataRecords,
  ...mystRecords,
  ...drawRecords,
  ...mathRecords,
  ...utilRecords,
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

router.beforeEach(async (to, from, next) => {
  await authGuard(to, from, async () => {
    await onlineGuard(to, from, next);
  });
});

export default router;
