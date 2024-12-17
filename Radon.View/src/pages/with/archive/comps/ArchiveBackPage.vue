<script lang="ts" setup>
import { useRouteQuery } from "@vueuse/router";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { useRouter } from "vue-router";

const { push } = useRouter();
const other = useRouteQuery("other");
const goBack = () => {
  if (other.value) {
    history.back();
  } else {
    push("/archive");
  }
};
</script>

<template>
  <t-button class="r-no-select" shape="circle" theme="primary" variant="text" @click="goBack">
    <ArrowLeftIcon />
  </t-button>
</template>
