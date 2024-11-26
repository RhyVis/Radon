<script setup lang="ts">
import ProjectIcon from "@/assets/icon.svg";
import { darkModeKey } from "@/lib/symbol/sharedSymbols";
import { useGlobalStore } from "@/store/global";
import { useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MenuFoldIcon, MenuUnfoldIcon, MoonIcon, SunnyIcon, UserIcon } from "tdesign-icons-vue-next";
import { computed, inject } from "vue";
import { useRouter } from "vue-router";

const dev = computed(() => import.meta.env.DEV);
const router = useRouter();
const global = useGlobalStore();
const dark = inject(darkModeKey)!;
const toggleDark = useToggle(dark);
const { asideVisible, authPassed } = storeToRefs(global);

const handleAside = () => (asideVisible.value = !asideVisible.value);
const handleAuth = () => router.push("/auth");
</script>

<template>
  <t-head-menu class="r-ct-hd-icon-margin">
    <template #logo>
      <div class="r-no-select" @click="handleAside">
        <t-image shape="round" :src="ProjectIcon" alt="Radon" style="width: 30px; height: 30px" />
      </div>
    </template>
    <t-divider layout="vertical" />
    <div class="r-ct-hd-content">
      <t-space>
        <div>
          <t-space :size="4">
            <span class="r-ct-hd-content-tt1">Radon</span>
            <i class="r-ct-hd-content-tt tw-font-thin">Gen3</i>
          </t-space>
        </div>
        <div v-if="dev">
          <i class="text-primary r-ct-hd-content-dev"> Development Mode </i>
        </div>
      </t-space>
    </div>
    <template #operations>
      <t-space class="r-ct-hd-operations" :size="6">
        <!-- Auth -->
        <t-button v-if="authPassed" theme="default" variant="outline" shape="circle" @click="handleAuth">
          <UserIcon />
        </t-button>
        <!-- Locale -->
        <sel-locale />
        <!-- Dark -->
        <t-button theme="default" variant="outline" shape="circle" @click="toggleDark()">
          <MoonIcon v-if="dark" />
          <SunnyIcon v-else />
        </t-button>
        <!-- Sidebar -->
        <t-button theme="default" variant="outline" shape="circle" @click="handleAside">
          <template #icon>
            <MenuUnfoldIcon v-if="asideVisible" />
            <MenuFoldIcon v-else />
          </template>
        </t-button>
      </t-space>
    </template>
  </t-head-menu>
</template>

<style scoped lang="less">
@import "@/assets/style/mixin";

.r-ct-hd-icon-margin :deep(.t-menu__logo) {
  margin-right: 6px;
}
.r-ct-hd-content {
  margin: 0;
  .r-no-select();
  .r-ct-hd-content-tt1 {
    .r-pub-font-chain();
    font-weight: bold;
  }
  .r-ct-hd-content-tt {
    font-size: smaller;
  }
  .r-ct-hd-content-dev {
    font-size: x-small;
  }
}
.r-ct-hd-operations {
  margin-right: 4px;
}
</style>
