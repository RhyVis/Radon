<script setup lang="ts">
import type { PdxLangEventItem } from "@/pages/with/pdx-parser/scripts/define.ts";
import { replacePdxAlias } from "@/pages/with/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import type { SizeEnum } from "tdesign-vue-next";

const { eventSelectId, replacer } = storeToRefs(usePdxStore());

const { eventResult } = defineProps<{
  eventResult: PdxLangEventItem[];
}>();

const visible = defineModel<boolean>("visible", { required: true, default: false });

const baseLayoutEl = document.getElementById("base-content");

const textAlias = (raw: string) => replacePdxAlias(raw, get(replacer));
const scrollTop = () => {
  if (baseLayoutEl) {
    baseLayoutEl.scrollTop = 0;
  }
};
const handleSwitch = (id: number) => {
  if (id < 0 || id >= eventResult.length) {
    return;
  }
  scrollTop();
  set(eventSelectId, id);
  set(visible, false);
};
</script>

<template>
  <t-drawer v-model:visible="visible" :footer="false">
    <t-list :scroll="{ type: 'virtual' }" :stripe="true" size="small">
      <t-list-item v-for="(event, eventKey) in eventResult" :key="eventKey">
        <span>{{ textAlias(event.name) }}</span>
        <template #action>
          <t-button :size="'extra-small' as SizeEnum" shape="circle" variant="outline" @click="handleSwitch(eventKey)">
            {{ eventKey }}
          </t-button>
        </template>
      </t-list-item>
    </t-list>
  </t-drawer>
</template>

<style scoped></style>
