<script setup lang="ts">
import { useGlobalStore } from "@/store/global";
import { set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import type { MenuRoute, MenuValue } from "tdesign-vue-next";
import type { RouteRecordRaw } from "vue-router";

defineProps<{
  name: string;
  iconName: string;
  records: RouteRecordRaw[];
}>();

const { authPassed, asideVisible } = storeToRefs(useGlobalStore());

const handleClose = () => {
  set(asideVisible, false);
};
</script>

<template>
  <t-submenu :value="name.toLowerCase()">
    <template #icon>
      <t-icon :name="iconName" />
    </template>
    <template #title>
      <span>{{ name }}</span>
    </template>
    <div v-for="item in records" :key="item.name">
      <t-menu-item
        v-if="!(item.meta!.auth && !authPassed)"
        :to="item.path as MenuRoute"
        :value="item.name as MenuValue"
        @click="handleClose"
      >
        <template #icon>
          <t-icon v-if="item.meta!.icon" :name="item.meta!.icon" />
        </template>
        <span>{{ item.meta!.title }}</span>
      </t-menu-item>
    </div>
  </t-submenu>
</template>

<style scoped></style>
