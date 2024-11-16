<script setup lang="ts">
import ProjectIcon from "@/assets/icon.svg";
import SideMenuElement from "@/layout/particial/SideMenuElement.vue";
import { dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { useGlobalStore } from "@/store/global";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { CloseIcon, FileUnknownIcon, HomeIcon } from "tdesign-icons-vue-next";
import type { MenuRoute } from "tdesign-vue-next";
import { onMounted } from "vue";

const global = useGlobalStore();
const { asideVisible } = storeToRefs(global);

const handleClose = () => {
  set(asideVisible, false);
};

onMounted(() => {
  handleClose();
});
</script>

<template>
  <t-drawer
    class="r-ct-sd r-drawer-no-padding"
    v-model:visible="asideVisible"
    placement="left"
    size="232px"
    :footer="false"
  >
    <t-menu class="r-ct-sd-ht" :expand-mutex="true">
      <!-- Head -->
      <template #logo>
        <t-image class="r-ct-icon" shape="round" :src="ProjectIcon" alt="Radon" />
        <span class="r-ct-sd-tt">Radon</span>
      </template>
      <t-menu-item :to="'/' as MenuRoute" @click="handleClose">
        <template #icon>
          <HomeIcon />
        </template>
        <span>HomePage</span>
      </t-menu-item>

      <!--Data-->
      <SideMenuElement name="Data" icon-name="data-base" :records="dataRecords" />
      <!--Myst-->
      <SideMenuElement name="Myst" icon-name="relation" :records="mystRecords" />
      <!--Draw-->
      <SideMenuElement name="Draw" icon-name="pen-brush" :records="drawRecords" />
      <!--Math-->
      <SideMenuElement name="Math" icon-name="numbers-0-1" :records="mathRecords" />
      <!--Util-->
      <SideMenuElement name="Util" icon-name="tools" :records="utilRecords" />

      <t-submenu value="extra">
        <template #icon>
          <FileUnknownIcon />
        </template>
        <template #title>
          <span>Extra</span>
        </template>
        <t-menu-item :to="'/credits' as MenuRoute" value="credits" @click="handleClose">
          <template #icon>
            <t-icon name="undertake-delivery" />
          </template>
          <span>Credits</span>
        </t-menu-item>
        <t-menu-item v-if="global.authShow" :to="'/auth' as MenuRoute" value="auth" @click="handleClose">
          <template #icon>
            <t-icon name="lock-on" />
          </template>
          <span>Auth</span>
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
