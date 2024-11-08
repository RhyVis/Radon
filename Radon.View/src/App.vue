<script lang="ts" setup>
import ContentFooter from "@/layout/basic/BaseLayoutFooter.vue";
import ContentHeader from "@/layout/basic/BaseLayoutHeader.vue";
import ContentSide from "@/layout/basic/BaseLayoutSide.vue";
import { authValidateWithRefresh } from "@/lib/common/authMethods";
import { loadFonts } from "@/lib/util/fontLoader";
import { useGlobalStore } from "@/store/global";
import { MessagePlugin } from "tdesign-vue-next";

const global = useGlobalStore();
const router = useRouter();
const i18n = useI18n();
const { t } = i18n;

window.onload = () => {
  document.addEventListener("touchstart", function (event) {
    if (event.touches.length > 1) {
      event.preventDefault();
    }
  });
  document.addEventListener("gesturestart", function (event) {
    event.preventDefault();
  });
  let lastTouchEnd = 0;
  document.documentElement.addEventListener(
    "touchend",
    event => {
      const now = Date.now();
      if (now - lastTouchEnd <= 300) {
        event.preventDefault();
      }
      lastTouchEnd = now;
    },
    {
      passive: false,
    },
  );
};

window.onresize = () => {
  if (document.body.style.zoom != "1") {
    console.debug(`Unexpected resize: ${document.body.style.zoom}`);
    document.body.style.zoom = "1.0";
  }
};

const syncLocale = () => {
  i18n.locale.value = global.locale;
  console.info(`Locale synced: ${global.locale}`);
};
const tryLoadFonts = () => {
  try {
    if (!global.fontLoaded) {
      loadFonts().then(() => (global.fontLoaded = true));
    }
  } catch (e) {
    console.error(e);
    MessagePlugin.error(t("loader.message.fontLoadingFailed"));
  }
};
const tryRefreshToken = () => {
  if (global.authPassed) {
    authValidateWithRefresh().then(v => {
      if (v) {
        MessagePlugin.success(t("auth.message.tokenValidAndRefreshed"));
      } else {
        global.authPassed = false;
        MessagePlugin.error(t("auth.message.tokenInvalid"));
        setTimeout(() => {
          router.push("/auth");
        }, 1420);
      }
    });
  }
};

onMounted(() => {
  syncLocale();
  tryLoadFonts();
  tryRefreshToken();
});

watch(
  () => router.currentRoute.value.path,
  () => {
    const title = router.currentRoute.value.meta.title as string | null;
    if (title) {
      useTitle(title);
    }
  },
);
watch(
  () => global.locale,
  () => {
    i18n.locale.value = global.locale;
  },
);
</script>

<template>
  <base-layout class="r-app-base-layout">
    <template #side>
      <ContentSide />
    </template>
    <template #header>
      <ContentHeader />
    </template>
    <template #default>
      <RouterView v-slot="{ Component }">
        <Transition name="route" mode="out-in">
          <Component :is="Component" />
        </Transition>
      </RouterView>
    </template>
    <template #footer>
      <ContentFooter />
    </template>
  </base-layout>
</template>

<style scoped lang="less">
.r-app-base-layout {
  height: 100vh;
}

.route-enter-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}
.route-leave-active {
  transition: all 0.42s cubic-bezier(1, 0.5, 0.8, 1);
}
.route-enter-from,
.route-leave-to {
  transform: translateX(-20px);
  opacity: 0;
}
</style>
