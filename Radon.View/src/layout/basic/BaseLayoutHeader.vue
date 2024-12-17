<script lang="ts" setup>
import ProjectIcon from "@/assets/icon.svg";
import { darkModeKey } from "@/lib/symbol/sharedSymbols";
import { useGlobalStore } from "@/store/global";
import { useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { MenuFoldIcon, MenuUnfoldIcon, MoonIcon, SunnyIcon, UserIcon } from "tdesign-icons-vue-next";
import { computed, inject } from "vue";
import { useRouter } from "vue-router";

const { push } = useRouter();
const { sideVisible, authPassed } = storeToRefs(useGlobalStore());
const dev = computed(() => import.meta.env.DEV ?? false);
const dark = inject(darkModeKey)!;
const toggleDark = useToggle(dark);
const toggleSide = useToggle(sideVisible);

const handleSide = () => toggleSide();
const handleAuth = () => push("/auth");
</script>

<template>
  <t-head-menu class="r-ct-hd-icon-margin">
    <template #logo>
      <div class="r-no-select" @click="handleSide">
        <t-image :src="ProjectIcon" alt="Radon" shape="round" style="width: 30px; height: 30px" />
      </div>
    </template>
    <t-divider layout="vertical" />
    <div class="r-ct-hd-content">
      <t-space>
        <div>
          <t-space :size="4">
            <span class="r-ct-hd-content-tt1">Radon</span>
            <i class="r-ct-hd-content-tt font-thin">Gen3</i>
          </t-space>
        </div>
        <div v-if="dev">
          <i class="text-primary r-ct-hd-content-dev text-blue-500"> Development Mode </i>
        </div>
      </t-space>
    </div>
    <template #operations>
      <t-space :size="6" class="r-ct-hd-operations">
        <!-- Auth -->
        <t-button v-if="authPassed" shape="circle" theme="default" variant="outline" @click="handleAuth">
          <UserIcon />
        </t-button>
        <!-- Locale -->
        <sel-locale />
        <!-- Dark -->
        <t-button shape="circle" theme="default" variant="outline" @click="toggleDark()">
          <MoonIcon v-if="dark" />
          <SunnyIcon v-else />
        </t-button>
        <!-- Sidebar -->
        <t-button shape="circle" theme="default" variant="outline" @click="handleSide">
          <template #icon>
            <MenuUnfoldIcon v-if="sideVisible" />
            <MenuFoldIcon v-else />
          </template>
        </t-button>
      </t-space>
    </template>
  </t-head-menu>
</template>

<style lang="less" scoped>
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
