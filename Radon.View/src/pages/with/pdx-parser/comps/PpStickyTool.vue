<script lang="ts" setup>
import { PpStickyAction } from "@/pages/with/pdx-parser/scripts/define.ts";
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { get, onKeyStroke, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { ArrowDownIcon, ArrowUpIcon, ListIcon, RefreshIcon } from "tdesign-icons-vue-next";
import type { TdStickyItemProps } from "tdesign-vue-next";
import { computed, ref } from "vue";

const { eventSelectId } = storeToRefs(usePdxStore());

const {
  type,
  onLeft = false,
  eventResultLen = 0,
} = defineProps<{
  type: "event" | "tree";
  onLeft: boolean;
  eventResultLen?: number;
}>();

const menuReplaceVisible = defineModel<boolean>("menuReplaceVisible", { required: false, default: false });
const drawerEventVisible = defineModel<boolean>("drawerEventVisible", { required: false, default: false });

const typeEvent = computed(() => type === "event");
const baseLayoutEl = document.getElementById("base-content");

const placement = computed<"left-bottom" | "right-bottom">(() => (onLeft ? "left-bottom" : "right-bottom"));
const offset = ref([-50, 20]);

const scrollTop = () => {
  if (baseLayoutEl) {
    baseLayoutEl.scrollTop = 0;
  }
};
const handleStickyTool = (context: { e: MouseEvent; item: TdStickyItemProps }) => {
  const { label } = context.item;
  switch (label) {
    case PpStickyAction.REPLACE:
      set(menuReplaceVisible, true);
      break;
    case PpStickyAction.EVENT:
      set(drawerEventVisible, true);
      break;
    case PpStickyAction.EVENT_ID_PLUS:
      handleEventPlus();
      break;
    case PpStickyAction.EVENT_ID_MINUS:
      handleEventMinus();
      break;
  }
};
const handleEventPlus = () => {
  if (get(typeEvent) && get(eventSelectId) < eventResultLen - 1) {
    scrollTop();
    eventSelectId.value++;
  }
};
const handleEventMinus = () => {
  if (get(typeEvent) && get(eventSelectId) > 0) {
    scrollTop();
    eventSelectId.value--;
  }
};

onKeyStroke("ArrowUp", handleEventMinus);
onKeyStroke("ArrowLeft", handleEventMinus);
onKeyStroke("ArrowDown", handleEventPlus);
onKeyStroke("ArrowRight", handleEventPlus);
</script>

<template>
  <t-space>
    <t-sticky-tool :offset="offset" :placement="placement" shape="round" type="compact" @click="handleStickyTool">
      <t-sticky-item :label="PpStickyAction.REPLACE">
        <template #icon>
          <RefreshIcon />
        </template>
      </t-sticky-item>
      <template v-if="typeEvent">
        <t-sticky-item :label="PpStickyAction.EVENT">
          <template #icon>
            <ListIcon />
          </template>
        </t-sticky-item>
        <t-sticky-item :label="PpStickyAction.EVENT_ID_MINUS">
          <template #icon>
            <ArrowUpIcon />
          </template>
        </t-sticky-item>
        <t-sticky-item :label="PpStickyAction.EVENT_ID_PLUS">
          <template #icon>
            <ArrowDownIcon />
          </template>
        </t-sticky-item>
      </template>
    </t-sticky-tool>
  </t-space>
</template>
