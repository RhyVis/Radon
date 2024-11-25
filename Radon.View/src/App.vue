<script lang="ts" setup>
import { useLoader } from "@/composable/useLoader";
import ContentFooter from "@/layout/basic/BaseLayoutFooter.vue";
import ContentHeader from "@/layout/basic/BaseLayoutHeader.vue";
import ContentSide from "@/layout/basic/BaseLayoutSide.vue";
import { authValidateWithRefresh } from "@/lib/common/authMethods";
import { fontLoaderKey } from "@/lib/symbol/loaderSymbols";
import { darkModeKey } from "@/lib/symbol/sharedSymbols";
import { getFontLoaders } from "@/lib/util/fontLoader";
import { changeMetaColor } from "@/lib/util/themeUtil";
import { useGlobalStore } from "@/store/global";
import { useVersionStore } from "@/store/version.ts";
import { get, set, syncRef, useDark, useIdle, useTitle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, provide, watch } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const version = useVersionStore();
const global = useGlobalStore();
const router = useRouter();
const i18n = useI18n();
const { t } = i18n;
const { locale, authPassed } = storeToRefs(global);

const fontLoader = useLoader(getFontLoaders(), "FontLoader");
const dark = useDark({
  onChanged: v => {
    if (v) {
      document.documentElement.setAttribute("theme-mode", "dark");
      changeMetaColor("#1f2937");
    } else {
      document.documentElement.removeAttribute("theme-mode");
      changeMetaColor("#ffffff");
    }
  },
});
const { idle } = useIdle();

const tryLoadFonts = () => {
  if (!get(fontLoader.completed)) {
    fontLoader.load();
  }
};
const tryRefreshToken = () => {
  if (get(authPassed)) {
    authValidateWithRefresh().then(v => {
      if (v) {
        MessagePlugin.success(t("auth.message.tokenValidAndRefreshed"));
      } else {
        set(authPassed, false);
        MessagePlugin.warning(t("auth.message.tokenInvalid"));
        setTimeout(() => {
          router.push("/auth");
        }, 1420);
      }
    });
  }
};
const updateTitle = () => {
  const title = router.currentRoute.value.meta.title as string | null;
  if (title) {
    useTitle(title);
  }
};

// Provide & sync section

syncRef(locale, i18n.locale);

provide(fontLoaderKey, fontLoader);
provide(darkModeKey, dark);

// Hook section

onMounted(() => {
  version.init();
  tryLoadFonts();
  tryRefreshToken();
});

watch(
  () => router.currentRoute.value.path,
  () => updateTitle(),
  { deep: false },
);
watch(
  () => idle.value,
  v => {
    if (v) {
      useTitle("...Zzz");
    } else {
      updateTitle();
    }
  },
  { immediate: true, deep: false },
);
</script>

<template>
  <base-layout class="r-app-base-layout">
    <template #left>
      <ContentSide />
    </template>
    <template #header>
      <ContentHeader />
    </template>
    <template #default>
      <prompt-update />
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
  transition: all 0.2s cubic-bezier(1, 0.5, 0.8, 1);
}
.route-leave-active {
  transition: all 0.16s cubic-bezier(1, 0.5, 0.8, 1);
}
.route-enter-from,
.route-leave-to {
  transform: translateX(-20px);
  opacity: 0;
}
</style>
