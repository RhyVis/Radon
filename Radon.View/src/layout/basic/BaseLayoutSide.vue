<script lang="ts" setup>
import ProjectIcon from "@/assets/icon.svg";
import SideMenuGroup from "@/layout/particial/SideMenuGroup.vue";
import { dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { useGlobalStore } from "@/store/global";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { CloseIcon, CollectionIcon, File1Icon, FileUnknownIcon, HomeIcon } from "tdesign-icons-vue-next";
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
    v-model:visible="asideVisible"
    :footer="false"
    class="r-ct-sd r-drawer-no-padding"
    placement="left"
    size="232px"
  >
    <t-menu v-model="menuActive" :expand-mutex="true" class="r-ct-sd-ht">
      <!-- Head -->
      <template #logo>
        <t-image :src="ProjectIcon" alt="Radon" class="r-ct-icon" shape="round" />
        <span class="r-ct-sd-tt">Radon</span>
      </template>
      <t-menu-item :to="'/' as MenuRoute" value="home" @click="handleClose">
        <template #icon>
          <HomeIcon />
        </template>
        <span>{{ t("route.home") }}</span>
      </t-menu-item>

      <t-menu-item :to="'/archive' as MenuRoute" value="archive" @click="handleClose">
        <template #icon>
          <File1Icon />
        </template>
        <span>{{ t("route.archive") }}</span>
      </t-menu-item>

      <!--Data-->
      <SideMenuGroup :records="dataRecords" icon-name="data-base" name="Data" name-key="route.data.title" />
      <!--Myst-->
      <SideMenuGroup :records="mystRecords" icon-name="relation" name="Myst" name-key="route.myst.title" />
      <!--Draw-->
      <SideMenuGroup :records="drawRecords" icon-name="pen-brush" name="Draw" name-key="route.draw.title" />
      <!--Math-->
      <SideMenuGroup :records="mathRecords" icon-name="numbers-0-1" name="Math" name-key="route.math.title" />
      <!--Util-->
      <SideMenuGroup :records="utilRecords" icon-name="tools" name="Util" name-key="route.util.title">
        <t-menu-item :to="'/pdx-parser' as MenuRoute" value="pdx-parser" @click="handleClose">
          <template #icon>
            <CollectionIcon />
          </template>
          <span>{{ t("route.util.pdxParser") }}</span>
        </t-menu-item>
      </SideMenuGroup>

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
        <t-button shape="circle" theme="default" variant="outline" @click="handleClose">
          <CloseIcon />
        </t-button>
      </template>
    </t-menu>
  </t-drawer>
</template>

<style lang="less" scoped>
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
