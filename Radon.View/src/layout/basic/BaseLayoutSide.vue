<script setup lang="ts">
import ProjectIcon from "@/assets/icon.svg";
import SideMenuGroup from "@/layout/particial/SideMenuGroup.vue";
import { dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { useGlobalStore } from "@/store/global";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { CloseIcon, File1Icon, FileUnknownIcon, HomeIcon } from "tdesign-icons-vue-next";
import type { MenuRoute } from "tdesign-vue-next";
import { onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { asideVisible, authShow } = storeToRefs(useGlobalStore());
const menuActive = ref("home");
const handleClose = () => set(asideVisible, false);

onMounted(handleClose);
</script>

<template>
  <t-drawer
    class="r-ct-sd r-drawer-no-padding"
    v-model:visible="asideVisible"
    placement="left"
    size="232px"
    :footer="false"
  >
    <t-menu class="r-ct-sd-ht" v-model="menuActive" :expand-mutex="true">
      <!-- Head -->
      <template #logo>
        <t-image class="r-ct-icon" shape="round" :src="ProjectIcon" alt="Radon" />
        <span class="r-ct-sd-tt">Radon</span>
      </template>
      <t-menu-item :to="'/' as MenuRoute" value="home" @click="handleClose">
        <template #icon>
          <HomeIcon />
        </template>
        <span>{{ t("route.home") }}</span>
      </t-menu-item>

      <t-menu-item :to="'/md' as MenuRoute" value="md" @click="handleClose">
        <template #icon>
          <File1Icon />
        </template>
        <span>{{ t("route.archive") }}</span>
      </t-menu-item>

      <!--Data-->
      <SideMenuGroup name="Data" icon-name="data-base" name-key="route.data.title" :records="dataRecords" />
      <!--Myst-->
      <SideMenuGroup name="Myst" icon-name="relation" name-key="route.myst.title" :records="mystRecords" />
      <!--Draw-->
      <SideMenuGroup name="Draw" icon-name="pen-brush" name-key="route.draw.title" :records="drawRecords" />
      <!--Math-->
      <SideMenuGroup name="Math" icon-name="numbers-0-1" name-key="route.math.title" :records="mathRecords" />
      <!--Util-->
      <SideMenuGroup name="Util" icon-name="tools" name-key="route.util.title" :records="utilRecords" />

      <t-submenu value="extra">
        <template #icon>
          <FileUnknownIcon />
        </template>
        <template #title>
          <span>{{ t("route.extras") }}</span>
        </template>
        <t-menu-item :to="'/credits' as MenuRoute" value="credits" @click="handleClose">
          <template #icon>
            <t-icon name="undertake-delivery" />
          </template>
          <span>{{ t("route.credits") }}</span>
        </t-menu-item>
        <t-menu-item v-if="authShow" :to="'/auth' as MenuRoute" value="auth" @click="handleClose">
          <template #icon>
            <t-icon name="lock-on" />
          </template>
          <span>{{ t("route.auth") }}</span>
        </t-menu-item>
      </t-submenu>

      <template #operations>
        <t-button theme="default" shape="circle" variant="outline" @click="handleClose">
          <CloseIcon />
        </t-button>
      </template>
    </t-menu>
  </t-drawer>
</template>

<style scoped lang="less">
@import "@/assets/style/mixin";

:global(.r-drawer-no-padding .t-drawer__body) {
  padding: 0;
}

.r-ct-sd {
  .r-ct-sd-ht {
    height: 100vh;
  }
  .r-ct-icon {
    width: 30px;
    height: 30px;
  }
  .r-ct-sd-tt {
    font-size: larger;
    font-weight: bold;
    .r-pub-font-chain();
    .r-no-select();
  }
}
</style>
