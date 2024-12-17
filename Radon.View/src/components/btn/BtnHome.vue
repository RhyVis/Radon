<script lang="ts" setup>
import { useGlobalStore } from "@/store/global.ts";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { HomeIcon } from "tdesign-icons-vue-next";
import { useRouter } from "vue-router";

const { push } = useRouter();
const { sideValue } = storeToRefs(useGlobalStore());
const backHome = () => {
  push("/");
  set(sideValue, "home");
};
</script>

<template>
  <t-button class="r-no-select" shape="circle" theme="primary" variant="text" @click="backHome">
    <HomeIcon />
  </t-button>
</template>
