<script lang="ts" setup>
import ProjectIcon from "@/assets/icon.svg";
import SideMenuEntryPrimitive from "@/layout/particial/SideMenuEntryPrimitive.vue";
import SideMenuGroup from "@/layout/particial/SideMenuGroup.vue";
import SideMenuSub from "@/layout/particial/SideMenuSub.vue";
import { dataRecords, drawRecords, mathRecords, mystRecords, utilRecords } from "@/router/records";
import { useGlobalStore } from "@/store/global";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { CloseIcon } from "tdesign-icons-vue-next";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const { push } = useRouter();
const { asideVisible, authShow } = storeToRefs(useGlobalStore());
const menuActive = ref("home");
const handleClose = (url?: string) => {
  set(asideVisible, false);
  if (url) push(url);
};

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

      <SideMenuEntryPrimitive icon="home" title-key="route.home" to="/" value="home" />

      <SideMenuEntryPrimitive icon="file-1" title-key="route.archive" to="/archive" value="archive" />

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
        <SideMenuEntryPrimitive
          icon="collection"
          title-key="route.util.pdxParser"
          to="/pdx-parser"
          value="pdx-parser"
        />
      </SideMenuGroup>

      <SideMenuSub icon="file-unknown" title-key="route.extras" value="extras">
        <SideMenuEntryPrimitive icon="undertake-delivery" title-key="route.credits" to="/credits" value="credits" />
        <SideMenuEntryPrimitive v-if="authShow" icon="lock-on" title-key="route.auth" to="/auth" value="auth" />
      </SideMenuSub>

      <template #operations>
        <t-button shape="circle" theme="default" variant="outline" @click="handleClose()">
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
