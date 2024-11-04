import { validateToken } from "@/lib/util/authFunction";
import { useAuthStore } from "@/pages/base/auth/scripts/store";
import HomeView from "@/pages/base/home/index.vue";
import { baseRecords, dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { MessagePlugin } from "tdesign-vue-next";
import type { RouteRecordRaw } from "vue-router";
import { createRouter, createWebHistory } from "vue-router";

const records: RouteRecordRaw[] = [
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
  scrollBehavior(_to, _from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    } else {
      return { top: 0 };
    }
  },
});

router.beforeEach(async (to, _, next) => {
  if (to.meta.auth) {
    const auth = useAuthStore();
    const { token } = auth;
    if (token.length === 0) {
      next("/error");
      await MessagePlugin.warning("Token不存在");
    } else {
      const r = await validateToken(token);
      if (r) {
        next();
      } else {
        next("/error");
      }
    }
  } else {
    next();
  }
});

export default router;
