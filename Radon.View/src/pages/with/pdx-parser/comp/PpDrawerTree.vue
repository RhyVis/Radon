<script setup lang="ts">
import type { TreeNodeValue, TreeProps } from "tdesign-vue-next";

const visible = defineModel<boolean>("visible", { required: true, default: false });
const treeVal = defineModel<TreeNodeValue[]>("treeVal", { required: true, default: [] });
const resultTree = defineModel<TreeProps["data"]>("resultTree", { required: true, default: [] });
</script>

<template>
  <t-drawer v-model:visible="visible" :footer="false">
    <t-card>
      <t-tree
        v-model:actived="treeVal"
        :activable="true"
        :data="resultTree"
        :hover="true"
        :line="true"
        :transition="true"
        @click="visible = false"
      />
    </t-card>
  </t-drawer>
</template>

<style scoped></style>
