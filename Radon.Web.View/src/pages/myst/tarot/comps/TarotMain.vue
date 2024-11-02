<script lang="tsx" setup>
import type { CardDisplay } from "@/lib/type/typeTarot";
import { intToRoman } from "@/pages/math/roman/scripts/romanNum";
import { MoonIcon, SunnyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed } from "vue";

const { card, index } = defineProps<{
  card: CardDisplay;
  index: number;
}>();

const rev = computed(() => card.data.rev);
const revClass = computed(() => ({ "r-tarot-img-rev": rev.value }));
const revText = computed(() => (rev.value ? "(逆位)" : "(正位)"));
const revDesc = computed(() => (rev.value ? card.data.desc.reverse : card.data.desc.upright));
const indexNum = computed(() => intToRoman(index + 1));

const handleImage = () => {
  // eslint-disable-next-line vue/no-mutating-props
  card.showImg = !card.showImg;
};
const handleImageErr = (name: string) => {
  MessagePlugin.error(`加载图片失败: ${name}`);
};
const handleDesc = () => {
  // eslint-disable-next-line vue/no-mutating-props
  card.showDesc = !card.showDesc;
};
const handleHash = () => {
  location.hash = `tarot-desc-${index}`;
};
</script>

<template>
  <t-card class="r-tarot-main-card-override mb-2" :header-bordered="true" :id="`tarot-main-${index}`">
    <template #title>
      <div class="text-primary r-no-select" @click="handleImage">
        {{ card.data.loc }}
      </div>
    </template>
    <template #actions>
      <div @click="handleHash">
        <i class="small text-black-50 r-no-select">{{ indexNum }} - {{ revText }} - </i>
        <MoonIcon v-if="rev" />
        <SunnyIcon v-else />
      </div>
    </template>
    <div v-show="card.showImg">
      <div class="r-tarot-main-div-full" @click="handleDesc">
        <div v-show="!card.showDesc">
          <t-image
            class="r-tarot-img-fit"
            :alt="card.data.name"
            :class="revClass"
            :lazy="true"
            :src="card.data.img"
            :style="{ width: 'fit-content', maxWidth: '100%', height: '360px' }"
            fit="scale-down"
            loading="💫"
            @error="handleImageErr(card.data.name)"
          />
        </div>
        <div v-show="card.showDesc" style="text-align: left; align-items: center">
          <t-title :content="revDesc" level="h6" />
          <t-text :content="card.data.desc.desc.join('')" />
        </div>
      </div>
    </div>
  </t-card>
</template>

<style scoped>
.r-no-select {
  user-select: none;
}

.r-tarot-img-rev {
  transform: rotate(180deg);
}

.r-tarot-img-fit {
  margin: auto;
}

.r-tarot-main-div-full {
  flex: 1;
  height: 360px;
  text-align: center;
}

.r-tarot-main-card-override :deep(.t-card__body) {
  height: 400px;
}
</style>
