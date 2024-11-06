<script setup lang="ts">
import { intToRoman } from "@/pages/math/roman/scripts/romanNum";
import type { CardDisplay } from "@/pages/myst/tarot/scripts/typeTarot";
import { computed } from "vue";

const { card, index } = defineProps<{
  card: CardDisplay;
  index: number;
}>();

const indexNum = computed(() => intToRoman(index + 1));
const revText = computed(() => (card.data.rev ? "逆位" : "正位"));
const revDesc = computed(() => (card.data.rev ? card.data.desc.reverse : card.data.desc.upright));

const handleHash = () => {
  location.hash = `tarot-main-${index}`;
};
</script>

<template>
  <t-card class="mb-2" :header-bordered="true" :id="`tarot-desc-${index}`">
    <template #title>
      <span @click="handleHash">{{ indexNum }}</span>
    </template>
    <template #subtitle>
      <t-space :size="4">
        <span>{{ card.data.loc }}</span>
        <i class="r-tr-desc-tt"> {{ card.data.name }}</i>
      </t-space>
    </template>
    <template #actions>
      <t-tag class="r-tr-desc-tag" @click="handleHash" theme="primary">{{ revText }}</t-tag>
    </template>
    <div>
      <t-title level="h6" :content="revDesc" />
      <t-paragraph v-if="card.data.desc.desc.length > 0">
        <t-text v-for="(line, index) in card.data.desc.desc" :key="index" :content="line" />
      </t-paragraph>
    </div>
  </t-card>
</template>

<style scoped>
.r-tr-desc-tt {
  font-size: xx-small;
}
.r-tr-desc-tag {
  font-weight: bold;
  user-select: none;
}
</style>
